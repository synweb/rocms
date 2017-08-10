using System.Security.Principal;
using System.Web.Mvc;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Contract.Models.Security
{
    public class RoPrincipal: IPrincipal
    {
        public int UserId { get; private set; }
        public IIdentity Identity { get; private set; }

        public RoPrincipal(int userId, IIdentity identity)
        {
            UserId = userId;
            Identity = identity;
        }


        public bool IsInRole(string role)
        {
            return false;
        }


        public bool IsAuthorizedForResource(string resource)
        {
            ISecurityService service = DependencyResolver.Current.GetService<ISecurityService>();
            return service.IsAuthorizedForResource(UserId, resource);
        }
    }
}
