using System.Collections.Generic;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface ISecurityService
    {
        void ChangePassword(string user, string oldPassword, string password);
        void SetPassword(int userId, string password);

        int RegisterUser(string username, string password, string email);

        bool Authenticate(string username, string password);

        IEnumerable<string> GetUsernames();

        void RemoveUser(string username);

        User GetUser(int id);
        User GetUserByEmail(string email);

        User GetUser(string username);
        ICollection<User> GetUsers();
        
        bool IsAuthorizedForResource(int userId, string resource);
        void GrantResource(int userId, string resourceName);
        void ForbidResource(int userId, string resourceName);
        string GetUsername(int userId);
        ICollection<CMSResource> GetResources();
        void SetResources(int userId, ICollection<int> resourceIds);
        void UpdateUser(User data);
    }
}
