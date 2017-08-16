using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Routing;
using JetBrains.Annotations;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Helpers;

namespace RoCMS.Base.ForWeb.Models
{
    public static class WebApiConfigHelper
    {
        public static void ApiRoute(HttpRouteCollection routes, string url, [AspMvcController] string controller, [AspMvcAction] string action, bool sessionRequired = false)
        {
            url = String.Format("{0}/{1}", ConstantStrings.WebApiExecutionPath, url);
            HttpRouteValueDictionary defaults = new HttpRouteValueDictionary();
            if (controller != null)
            {
                defaults["controller"] = controller;
            }
            if (action != null)
            {
                defaults["action"] = action;
            }
            IHttpRoute route = routes.CreateRoute(url, defaults, new Dictionary<string, object>());
            routes.Add(route.RouteTemplate, route);
            if (sessionRequired)
            {
                // в урле передаётся название параметра. превращаем его в регулярку.
                var urlRegex = $"^~/" + Regex.Replace(url, @"{.+?}", "(.+?)");
                ActionSessionHelper.RegisterUrlRegex(urlRegex);
            }
        }
    }
}
