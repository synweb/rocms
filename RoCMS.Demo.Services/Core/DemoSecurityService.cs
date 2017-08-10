using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Demo.Services.Core
{
    public class DemoSecurityService: ISecurityService
    {
        private List<User> _defaultUsers;
        private List<CMSResource> _defaultCMSResources;

        public DemoSecurityService()
        {
            try
            {
                var file = "users.xml";
                var xs = new XmlSerializer(typeof(List<User>));
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoData", file);
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    _defaultUsers = (List<User>) xs.Deserialize(fs);
                }
            }
            catch
            {
                _defaultUsers = new List<User>
                {
                    new User()
                    {
                        UserId = 1,
                        Username = "rocms",
                        Password = "ropasswd"
                    }
                };
            }
            try
            {
                var file = "cms_resources.xml";
                var xs = new XmlSerializer(typeof(List<CMSResource>));
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoData", file);
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    _defaultCMSResources = (List<CMSResource>) xs.Deserialize(fs);
                }
            }
            catch
            {
                _defaultCMSResources = new List<CMSResource>
                {
                    new CMSResource()
                    {
                        CmsResourceId = 15,
                        Name = "AdminPanel",
                        Description = "Админка"
                    },
                    new CMSResource()
                    {
                        CmsResourceId = 1,
                        Name = "Users",
                        Description = "Пользователи"
                    },
                    new CMSResource()
                    {
                        CmsResourceId = 2,
                        Name = "News",
                        Description = "Новости"
                    },
                    new CMSResource()
                    {
                        CmsResourceId = 3,
                        Name = "Gallery",
                        Description = "Галерея"
                    },
                    new CMSResource()
                    {
                        CmsResourceId = 4,
                        Name = "Albums",
                        Description = "Альбомы"
                    },
                    new CMSResource()
                    {
                        CmsResourceId = 5,
                        Name = "CommonSettings",
                        Description = "Настройка"
                    },
                    new CMSResource()
                    {
                        CmsResourceId = 6,
                        Name = "Menus",
                        Description = "Меню"
                    },
                    new CMSResource()
                    {
                        CmsResourceId = 7,
                        Name = "Pages",
                        Description = "Страницы"
                    },
                    new CMSResource()
                    {
                        CmsResourceId = 8,
                        Name = "Blocks",
                        Description = "Блоки"
                    },
                    new CMSResource()
                    {
                        CmsResourceId = 25,
                        Name = "DeleteObjects",
                        Description = "Удаление объектов"
                    }
                };
            }

        }

        private const string USERS_SESSION_KEY = "Users";
        private void InitSessionDataIfEmpty(HttpContext ctx)
        {
            if (ctx.Session[USERS_SESSION_KEY] == null)
            {
                ctx.Session[USERS_SESSION_KEY] = _defaultUsers.ToList();
            }
        }

        private List<User> GetSessionUsers(HttpContext ctx)
        {
            return (List<User>)ctx.Session[USERS_SESSION_KEY];
        }

        public void ChangePassword(string username, string oldPassword, string password)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            if(string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password));
            var users = GetSessionUsers(HttpContext.Current);
            var user =
                users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
            if(user == null)
                throw new ArgumentException(nameof(username));
            if(!user.Password.Equals(oldPassword))
                throw new ArgumentException(nameof(oldPassword));
            user.Password = password;
        }

        public void SetPassword(int userId, string password)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password));
            var users = GetSessionUsers(HttpContext.Current);
            var user = users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
                throw new ArgumentException(nameof(userId));
            user.Password = password;
        }

        public int RegisterUser(string username, string password, string email)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password));
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException(nameof(username));
            var users = GetSessionUsers(HttpContext.Current);
            var user = users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
            if (user != null)
                throw new ArgumentException(nameof(username));
            int id = users.Max(x => x.UserId) + 1;
            users.Add(new User()
            {
                Username = username,
                Password = password,
                Email = email,
                UserId = id,
                CreationDate = DateTime.UtcNow
            });
            return id;
        }

        public bool Authenticate(string username, string password)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            if (string.IsNullOrEmpty(password))
                return false;
            if (string.IsNullOrEmpty(username))
                return false;
            var users = GetSessionUsers(HttpContext.Current);
            return users.Any(x =>
                x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase)
                && x.Password.Equals(password, StringComparison.InvariantCulture));
        }

        public IEnumerable<string> GetUsernames()
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var users = GetSessionUsers(HttpContext.Current);
            return users.Select(x => x.Username);
        }

        public void RemoveUser(string username)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException(nameof(username));
            var users = GetSessionUsers(HttpContext.Current);
            var user =
                users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
            if (user == null)
                throw new ArgumentException(nameof(username));
            if (user.UserId == 1)
                throw new NotSupportedException();
            users.Remove(user);
        }

        public User GetUser(int id)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var users = GetSessionUsers(HttpContext.Current);
            return users.FirstOrDefault(x => x.UserId == id);
        }

        public User GetUserByEmail(string email)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var users = GetSessionUsers(HttpContext.Current);
            return users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public User GetUser(string username)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var users = GetSessionUsers(HttpContext.Current);
            return users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
        }

        public ICollection<User> GetUsers()
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var users = GetSessionUsers(HttpContext.Current);
            return users;
        }

        public bool IsAuthorizedForResource(int userId, string resource)
        {
            //InitSessionDataIfEmpty(HttpContext.Current);
            // всем пользователям по умолчанию открыты все ресурсы
            return _defaultCMSResources.Any(x => x.Name.Equals(resource, StringComparison.InvariantCultureIgnoreCase));
        }

        public void GrantResource(int userId, string resourceName)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            // всем пользователям по умолчанию открыты все ресурсы
        }

        public void ForbidResource(int userId, string resourceName)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            // всем пользователям по умолчанию открыты все ресурсы
        }

        public string GetUsername(int userId)
        {
            return GetUser(userId).Username;
        }

        public ICollection<CMSResource> GetResources()
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            return _defaultCMSResources;
        }

        public void SetResources(int userId, ICollection<int> resourceIds)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            // всем пользователям по умолчанию открыты все ресурсы
        }

        public void UpdateUser(User data)
        {
            InitSessionDataIfEmpty(HttpContext.Current);
            var users = GetSessionUsers(HttpContext.Current);
            var user = users.FirstOrDefault(x => x.UserId == data.UserId);
            if (user == null)
                return;
            // удаляем старого, добавляем нового. оставляем только пароль.
            data.Password = user.Password;
            users.Remove(user);
            users.Add(data);
        }
    }
}
