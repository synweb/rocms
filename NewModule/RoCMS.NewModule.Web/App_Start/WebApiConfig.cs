using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;
using JetBrains.Annotations;
using RoCMS.Helpers;

namespace RoCMS.News.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ApiRoute(config.Routes, "news/create", "NewsApi", "Create");
            ApiRoute(config.Routes, "news/{id}/update", "NewsApi", "Update");
            ApiRoute(config.Routes, "news/{id}/delete", "NewsApi", "Delete");

            ApiRoute(config.Routes, "news/{id}/comment/create", "NewsApi", "CreateComment");
            ApiRoute(config.Routes, "news/admin/comment/{id}/delete", "NewsApi", "AdminDeleteComment");
            ApiRoute(config.Routes, "news/comment/{id}/delete", "NewsApi", "DeleteComment");
            ApiRoute(config.Routes, "news/comment/moderate", "NewsApi", "ModerateComment");
            ApiRoute(config.Routes, "news/comment/edit", "NewsApi", "UpdateText");
        }


        static void ApiRoute(HttpRouteCollection routes, string url, [AspMvcController] string controller, [AspMvcAction] string action)
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
        }
    }
}
