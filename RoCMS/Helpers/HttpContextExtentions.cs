using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoCMS.Helpers
{
    public static class HttpContextExtensions
    {
        public static void RewritePathToAction(this HttpContext context, string actionName, string controllerName, object routeValues)
        {
            if (routeValues == null)
            {
                routeValues = new { @true = false }; // TransferRequest не позволяет сбросить QueryString(как и HttpContext.Request), но позволяет его заменить.
            }
            string url = new UrlHelper(context.Request.RequestContext).Action(actionName, controllerName, routeValues);
            if (HttpRuntime.UsingIntegratedPipeline) // IIS7+, IIS Express
            {
                context.Server.TransferRequest(url, false);
            }
            else // ASP.NET Development Server, etc
            {
                context.RewritePath(url, false);
                ((IHttpHandler)new MvcHttpHandler()).ProcessRequest(context);
            }
            context.ApplicationInstance.CompleteRequest();
        }
    }
}