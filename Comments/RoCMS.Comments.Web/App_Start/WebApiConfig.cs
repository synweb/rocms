using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using JetBrains.Annotations;
using RoCMS.Helpers;

namespace RoCMS.Comments.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ApiRoute(config.Routes, "comment/{id}/delete", "CommentApi", "Delete");
            ApiRoute(config.Routes, "comment/{id}/hide", "CommentApi", "Hide");
            ApiRoute(config.Routes, "comment/{id}/show", "CommentApi", "Show");
            ApiRoute(config.Routes, "comment/create", "CommentApi", "Create");
        }


        static void ApiRoute(HttpRouteCollection routes, string url, [AspMvcController] string controller, [AspMvcAction] string action)
        {
            url = String.Format("{0}/{1}/{2}", ConstantStrings.WebApiExecutionPath, "comments", url);
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
