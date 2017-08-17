using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace RoCMS.FAQ.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            IEnumerable<string> controllerNames = typeof(RouteConfig).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .Where(t => !t.IsAbstract)
                .Select(t => String.Format("^{0}$", t.Name.Replace("Controller", "")));

            string constraint = String.Join("|", controllerNames);

            routes.MapRoute(
                name: "DefaultFAQ",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = constraint }
            );
        }
    }
}