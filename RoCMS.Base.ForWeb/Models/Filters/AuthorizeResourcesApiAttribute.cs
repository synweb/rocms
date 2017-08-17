using System;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using RoCMS.Web.Contract.Models.Security;

namespace RoCMS.Base.ForWeb.Models.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeResourcesApiAttribute : AuthorizeAttribute
    {
        #region Fields

        readonly string[] _resources;

        #endregion

        #region Constructors

        public AuthorizeResourcesApiAttribute(params string[] resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }
            if (resources.Length == 0)
            {
                throw new ArgumentException("Empty array", "resources");
            }
            _resources = resources;
        }

        #endregion

        #region Methods

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            base.OnAuthorization(filterContext);

            RoPrincipal currentPrincipal = Thread.CurrentPrincipal as RoPrincipal;
            if (currentPrincipal != null && currentPrincipal.Identity.IsAuthenticated && _resources.Length > 0)
            {
                foreach (string resource in _resources)
                {
                    if (!currentPrincipal.IsAuthorizedForResource(resource))
                    {
                        filterContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                        break;
                    }
                }
            }
        }

        #endregion
    }
}