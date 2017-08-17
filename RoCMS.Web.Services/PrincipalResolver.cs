using System.Threading;
using RoCMS.Web.Contract.Models.Security;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class PrincipalResolver : IPrincipalResolver
    {
        public RoPrincipal GetCurrentUser()
        {
            return Thread.CurrentPrincipal as RoPrincipal;
        }

        public int GetUserId()
        {
            return GetCurrentUser().UserId;
        }

        public int? GetUserIdIfAuthenticated()
        {
            return GetCurrentUser() != null ? GetUserId() : (int?)null;
        }
    }
}
