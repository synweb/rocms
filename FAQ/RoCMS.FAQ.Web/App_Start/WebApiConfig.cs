using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;
using JetBrains.Annotations;
using RoCMS.Helpers;

namespace RoCMS.FAQ.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ApiRoute(config.Routes, "faq/question/{id}/accept", "QuestionApi", "Accept");
            ApiRoute(config.Routes, "faq/question/{id}/hide", "QuestionApi", "Hide");
            ApiRoute(config.Routes, "faq/question/{id}/delete", "QuestionApi", "Delete");
            ApiRoute(config.Routes, "faq/question/update", "QuestionApi", "Update");
            ApiRoute(config.Routes, "faq/question/update-send", "QuestionApi", "UpdateAndSend");
            ApiRoute(config.Routes, "faq/question/create", "QuestionApi", "Create");
            ApiRoute(config.Routes, "faq/question/ask", "QuestionApi", "Ask");
            ApiRoute(config.Routes, "faq/questions/sort", "QuestionApi", "Sort");
        }


        static void ApiRoute(HttpRouteCollection routes, string url, [AspMvcController] string controller, [AspMvcAction] string action)
        {
            url = $"{ConstantStrings.WebApiExecutionPath}/{url}";
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
