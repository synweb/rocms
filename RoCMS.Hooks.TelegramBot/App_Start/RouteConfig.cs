using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;


namespace RoCMS.Hooks.TelegramBot
{
    public static class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection routes)
        {

            IEnumerable<string> controllerNames = typeof(RouteConfig).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .Where(t => !t.IsAbstract)
                .Select(t => $"^{t.Name.Replace("Controller", "")}$");

            string constraint = String.Join("|", controllerNames);

            routes.MapRoute(
                name: "DefaultTelegramBot",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                constraints: new {controller = constraint}
                );


        }
    }
}