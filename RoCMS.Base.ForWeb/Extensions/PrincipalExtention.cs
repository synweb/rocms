using System.Security.Principal;
using RoCMS.Web.Contract.Models.Security;

namespace RoCMS.Base.ForWeb.Extensions
{
    public static class PrincipalExtensions
    {
        public static bool IsAuthorizedForResource(this IPrincipal principal, string resourcePath)
        {

            var roPrincipal = principal as RoPrincipal;
            bool result = false;
            if (roPrincipal != null)
            {
                result = roPrincipal.IsAuthorizedForResource(resourcePath);
            }

            return result;
        }
    }
}