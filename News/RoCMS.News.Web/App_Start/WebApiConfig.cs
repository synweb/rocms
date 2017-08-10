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
            ApiRoute(config.Routes, "news/news/create", "NewsApi", "Create");
            ApiRoute(config.Routes, "news/news/{id}/update", "NewsApi", "Update");
            ApiRoute(config.Routes, "news/news/{id}/delete", "NewsApi", "Delete");
            ApiRoute(config.Routes, "news/news/{id}/get", "NewsApi", "Get");
            ApiRoute(config.Routes, "news/news/filter", "NewsApi", "Filter");

            ApiRoute(config.Routes, "news/{id}/comment/create", "NewsApi", "CreateComment");
            ApiRoute(config.Routes, "news/admin/comment/{id}/delete", "NewsApi", "AdminDeleteComment");
            ApiRoute(config.Routes, "news/comment/{id}/delete", "NewsApi", "DeleteComment");
            //ApiRoute(config.Routes, "news/comment/moderate", "NewsApi", "ModerateComment");
            //ApiRoute(config.Routes, "news/comment/edit", "NewsApi", "UpdateText");

            ApiRoute(config.Routes, "news/categories/get", "NewsCategoryApi", "GetCategories");
            ApiRoute(config.Routes, "news/categories/order/update", "NewsCategoryApi", "UpdateSortOrder");
            ApiRoute(config.Routes, "news/category/create", "NewsCategoryApi", "Create");
            ApiRoute(config.Routes, "news/category/update", "NewsCategoryApi", "Update");
            ApiRoute(config.Routes, "news/category/{categoryId}/delete", "NewsCategoryApi", "Remove");


            ApiRoute(config.Routes, "news/blog/client/create", "NewsBlogClientApi", "CreateBlog");
            ApiRoute(config.Routes, "news/blog/client/update", "NewsBlogClientApi", "UpdateBlog");
            ApiRoute(config.Routes, "news/blog/client/post/create", "NewsBlogClientApi", "CreatePost");
            ApiRoute(config.Routes, "news/blog/client/post/update", "NewsBlogClientApi", "UpdatePost");
            ApiRoute(config.Routes, "news/blog/client/post/{newsId}/delete", "NewsBlogClientApi", "DeletePost");
            ApiRoute(config.Routes, "news/blog/client/free/{url}", "NewsBlogClientApi", "BlogUrlIsFree");

            ApiRoute(config.Routes, "news/blog/admin/update", "NewsBlogAdminApi", "UpdateBlog");
            ApiRoute(config.Routes, "news/blog/{blogId}/delete", "NewsBlogAdminApi", "Delete");

            ApiRoute(config.Routes, "news/blog/profile/update", "NewsBlogClientApi", "UpdateProfile");

            ApiRoute(config.Routes, "news/tag/pattern/get", "TagApi", "GetTagByPattern");

            ApiRoute(config.Routes, "news/settings/get", "NewsSettingsApi", "Get");
            ApiRoute(config.Routes, "news/settings/update", "NewsSettingsApi", "Update");

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
