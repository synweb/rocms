using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Web
{
    public class RouteConfig
    {
        static RouteConfig()
        {
            var service = DependencyResolver.Current.GetService<INewsSettingsService>();
            try
            {
                var settings = service.GetNewsSettings();
                BlogUrl = String.IsNullOrEmpty(settings.BlogUrl) ? "blog" : settings.BlogUrl;
            }
            catch
            {}
        }



        public static string BlogUrl = "blog";

        public static void RegisterRoutes(RouteCollection routes)
        {
            RoutingHelper.RegisterHeartRoute(routes, typeof(NewsItem), "News", "BlogSEF");
            //routes.MapRoute(
            //    name: "NewsItem",
            //    url: "News/{id}",
            //    defaults: new { controller = "News", action = "News" },
            //    constraints: new { id = @"\d+" });

            //routes.MapRoute(
            //    name: "NewsPage",
            //    url: "News/Page",
            //    defaults: new { controller = "Redirect", action = "Index", relativeUrl = "blog" });


            //routes.MapRoute(
            //    name: "BlogTagSearch",
            //    url: BlogUrl + "/tag/{tag}",
            //    defaults: new { controller = "News", action = "TagSearch" }
            //    );

            //routes.MapRoute(
            //    name: "UserBlogItem",
            //    url: "blogs/{blogUrl}/{newsUrl}",
            //    defaults: new { controller = "News", action = "UserBlogItem" },
            //    constraints: new { blogUrl = @"\S+", newsUrl = @"\S+" }
            //    );

            //routes.MapRoute(
            //    name: "BlogModuleSEF",
            //    url: BlogUrl + "/{*relativeUrl}",
            //    defaults: new { controller = "News", action = "BlogModuleSEF" },
            //    constraints: new { relativeUrl = @"\S+" }
            //    );

            //routes.MapRoute(
            //    name: "BlogItem",
            //    url: BlogUrl + "/{*relativeUrl}",
            //    defaults: new { controller = "News", action = "BlogSEF" },
            //    constraints: new { relativeUrl = @"\S+" }
            //    );

            //routes.MapRoute(
            //    name: "NewsCategoryItem",
            //    url: BlogUrl + "/{*relativeUrl}",
            //    defaults: new {controller = "News", action = "СategorySEF"},
            //    constraints: new {relativeUrl = @"\S+"}
            //    );            

            //routes.MapRoute(
            //    name: "BlogPersonal",
            //    url: "personal",
            //    defaults: new { controller = "Personal", action = "Index" }
            //    );

            //routes.MapRoute(
            //    name: "UserBlog",
            //    url: "blogs/{blogUrl}",
            //    defaults: new { controller = "News", action = "UserBlog" },
            //    constraints: new { blogUrl = @"\S+" }
            //    );



            //routes.MapRoute(
            //    name: "BlogCreate",
            //    url: "personal/newblog",
            //    defaults: new { controller = "Personal", action = "CreateBlog" }
            //    );
            //routes.MapRoute(
            //    name: "BlogEdit",
            //    url: "personal/editblog",
            //    defaults: new { controller = "Personal", action = "EditBlog" }
            //    );
            //routes.MapRoute(
            //    name: "BlogItemCreate",
            //    url: "personal/newpost",
            //    defaults: new { controller = "Personal", action = "CreateBlogItem" }
            //    );
            //routes.MapRoute(
            //    name: "BlogItemEdit",
            //    url: "personal/editpost/{id}",
            //    defaults: new { controller = "Personal", action = "EditBlogItem" },
            //    constraints: new { id = @"\d+" }
            //    );



            // *****

            IEnumerable<string> controllerNames = typeof(RouteConfig).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .Where(t => !t.IsAbstract)
                .Select(t => $"^{t.Name.Replace("Controller", "")}$");

            string constraint = string.Join("|", controllerNames);

            routes.MapRoute(
                name: "DefaultNews",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = constraint }
            );
        }
    }
}