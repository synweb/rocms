using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RoCMS.News.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "NewsItem",
                url: "News/{id}",
                defaults: new { controller = "News", action = "News" },
                constraints: new { id = @"\d+" });

            routes.MapRoute(
                name: "NewsPage",
                url: "News/Page",
                defaults: new {controller = "Redirect", action = "Index", relativeUrl = "Блог"});

            routes.MapRoute(
                name: "BlogItemOld",
                url: "blog/{relativeUrl}",
                defaults: new { controller = "News", action = "NewsSEF" }
            );

            routes.MapRoute(
                name: "BlogItem",
                url: "Блог/{relativeUrl}",
                defaults: new {controller = "News", action = "BlogSEF"}
                );

            routes.MapRoute(
                name: "BlogTagSearchOld",
                url: "blog/tag/{tag}",
                defaults: new { controller = "News", action = "TagSearchOld" }
            );

            routes.MapRoute(
                name: "BlogTagSearch",
                url: "Блог/tag/{tag}",
                defaults: new {controller = "News", action = "TagSearch"}
                );

            IEnumerable<string> controllerNames = typeof(RouteConfig).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .Where(t => !t.IsAbstract)
                .Select(t => String.Format("^{0}$", t.Name.Replace("Controller", "")));

            string constraint = String.Join("|", controllerNames);

            routes.MapRoute(
                name: "DefaultNews",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = constraint }
            );
        }
    }
}