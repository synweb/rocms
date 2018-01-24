using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using AutoMapper;
using RoCMS.Base.Exceptions;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;
using RoCMS.Web.Services.Resources;
using User = RoCMS.Data.Models.User;

namespace RoCMS.Web.Services
{
    public class SecurityService: BaseCoreService, ISecurityService
    {
        private readonly UserGateway _userGateway = new UserGateway();
        private readonly UserCMSResourceGateway _userCMSResourceGateway = new UserCMSResourceGateway();
        private readonly CMSResourceGateway _cmsResourceGateway = new CMSResourceGateway();


        public static string CalculateHash(string input)
        {
            var hashAlg = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            var hash = hashAlg.ComputeHash(inputBytes);
            int seed = BitConverter.ToInt32(hash, 0);
            var saltRnd = new Random(seed);
            var saltBytes = new byte[64];
            saltRnd.NextBytes(saltBytes);
            var saltBytes2 = new byte[68];
            saltRnd.NextBytes(saltBytes2);
            var hashPlusSaltBytes = new byte[saltBytes.Length + saltBytes2.Length + hash.Length];
            Array.Copy(saltBytes, hashPlusSaltBytes, saltBytes.Length);
            Array.Copy(hash, 0, hashPlusSaltBytes, saltBytes.Length, hash.Length);
            Array.Copy(saltBytes2, 0, hashPlusSaltBytes, hash.Length + saltBytes.Length, saltBytes2.Length);
            var saltedHash = hashAlg.ComputeHash(hashPlusSaltBytes);
            
            // Без этих двух строк результаты работы этой функции, скомпилированной с оптимизацией и без неё, на некоторых машинах различаются
            // (было замечено только на одной)
            // короче, это убирать нельзя
            var logService = DependencyResolver.Current.GetService<ILogService>();
            logService.TraceMessage($"Random seed for hashing: {seed}");
            //logService.TraceMessage($"Initial hash: {BytesToHex(hash)}");
            //logService.TraceMessage($"Salt 1: {BytesToHex(saltBytes)}");
            //logService.TraceMessage($"Salt 2: {BytesToHex(saltBytes2)}");
            //logService.TraceMessage($"Final hash: {BytesToHex(saltedHash)}");

            return BytesToHex(saltedHash);
        }

        private static string BytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        public void ChangePassword(string user, string oldPassword, string password)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException();
            }
            if (!Authenticate(user, oldPassword))
            {
                throw new AuthenticationException();
            }
            var hash = CalculateHash(password);
            int userId = _userGateway.SelectByUsername(user).UserId;
            _userGateway.UpdatePassword(userId, hash);
        }

        public void SetPassword(int userId, string password)
        {
            var user = _userGateway.SelectOne(userId);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            var hash = CalculateHash(password);
            _userGateway.UpdatePassword(userId, hash);
        }

        public int RegisterUser(string username, string password, string email)
        {
            var existing = _userGateway.SelectByUsername(username);
            if(existing != null)
            {
                throw new UserExistsException("Пользователь уже существует. Пожалуйста, авторизуйтесь.");
            }
            var hash = CalculateHash(password);
            int id = _userGateway.Insert(new User()
            {
                Username = username,
                Password = hash,
                Email = email
            });
            return id;
        }

        public bool Authenticate(string username, string password)
        {
            var hash = CalculateHash(password);
            return _userGateway.Authenticate(username, hash);
        }

        public IEnumerable<string> GetUsernames()
        {
            var users = _userGateway.Select();
            return users.Select(x => x.Username);
        }

        public void RemoveUser(string username)
        {
            var user = _userGateway.SelectByUsername(username);

            IPrincipalResolver principal = DependencyResolver.Current.GetService<IPrincipalResolver>();
            if(user.UserId == principal.GetUserId())
                throw new Exception("Вы не можете удалить себя");

            _userGateway.Delete(user.UserId);
        }

        public Contract.Models.User GetUser(int id)
        {
            var dataRes = _userGateway.SelectOne(id);
            var user = Mapper.Map<Contract.Models.User>(dataRes);
            return user;
        }

        public Contract.Models.User GetUser(string username)
        {
            var dataRes = _userGateway.SelectByUsername(username);
            var user = Mapper.Map<Contract.Models.User>(dataRes);
            return user;
        }

        public Contract.Models.User GetUserByEmail(string email)
        {
            var dataRes = _userGateway.SelectByEmail(email);
            var user = Mapper.Map<Contract.Models.User>(dataRes);
            return user;
        }

        public ICollection<Contract.Models.User> GetUsers()
        {
            var dataRes = _userGateway.Select();
            var res = Mapper.Map<ICollection<Contract.Models.User>>(dataRes);
            return res;
        }

        public bool IsAuthorizedForResource(int userId, string resource)
        {
            try
            {
                return _userCMSResourceGateway.CheckIfAuthorizedForResource(userId, resource);
            }
            catch
            {
                // при ошибке запрещаем доступ (например, нет доступа к БД)
                return false;
            }
        }

        public void GrantResource(int userId, string resourceName)
        {
            bool authorized = _userCMSResourceGateway.CheckIfAuthorizedForResource(userId, resourceName);
            if (authorized)
                return; // права и так есть. не нужно ничего делать.
            var resource = _cmsResourceGateway.SelectByName(resourceName);
            _userCMSResourceGateway.Insert(userId, resource.CmsResourceId);
        }

        public void ForbidResource(int userId, string resourceName)
        {
            var resource = _cmsResourceGateway.SelectByName(resourceName);
            _userCMSResourceGateway.Delete(userId, resource.CmsResourceId);
        }

        public string GetUsername(int userId)
        {
            var rec = _userGateway.SelectOne(userId);
            return rec.Username;
        }

        public ICollection<CMSResource> GetResources()
        {
            var dataRes = _cmsResourceGateway.Select();
            var res = Mapper.Map<ICollection<CMSResource>>(dataRes);
            return res;
        }

        public void SetResources(int userId, ICollection<int> resourceIds)
        {
            using(var ts = new TransactionScope())
            {
                var currentResources =
                    _userCMSResourceGateway.SelectByUser(userId).Select(x => x.CmsResourceId).ToList();
                foreach (var id in currentResources.Except(resourceIds))
                {
                    //ресурсы, которые надо удалить
                    _userCMSResourceGateway.Delete(userId, id);
                }
                foreach (var id in resourceIds.Except(currentResources))
                {
                    //ресурсы, которые надо добавить
                    _userCMSResourceGateway.Insert(userId, id);
                }
                ts.Complete();
            }
        }

        public void UpdateUser(Contract.Models.User user)
        {
            var dataUser = Mapper.Map<User>(user);
            _userGateway.Update(dataUser);
        }

        protected override int CacheExpirationInMinutes => 30;
    }
}
