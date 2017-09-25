using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Routing;
using JetBrains.Annotations;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Helpers;

namespace RoCMS.News.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            WebApiConfigHelper.ApiRoute(config.Routes, "news/news/create", "NewsApi", "Create");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/news/{id}/update", "NewsApi", "Update");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/news/{id}/delete", "NewsApi", "Delete");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/news/{id}/get", "NewsApi", "Get");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/news/get", "NewsApi", "GetNewsItems");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/news/filter", "NewsApi", "Filter");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/{id}/view", "NewsApi", "IncreaseViewCount", true);

            WebApiConfigHelper.ApiRoute(config.Routes, "news/{id}/comment/create", "NewsApi", "CreateComment");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/admin/comment/{id}/delete", "NewsApi", "AdminDeleteComment");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/comment/{id}/delete", "NewsApi", "DeleteComment");
            //WebApiConfigHelper.ApiRoute(config.Routes, "news/comment/moderate", "NewsApi", "ModerateComment");
            //WebApiConfigHelper.ApiRoute(config.Routes, "news/comment/edit", "NewsApi", "UpdateText");

            WebApiConfigHelper.ApiRoute(config.Routes, "news/categories/get", "NewsCategoryApi", "GetCategories");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/categories/order/update", "NewsCategoryApi", "UpdateSortOrder");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/category/create", "NewsCategoryApi", "Create");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/category/update", "NewsCategoryApi", "Update");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/category/{categoryId}/delete", "NewsCategoryApi", "Remove");


            WebApiConfigHelper.ApiRoute(config.Routes, "news/blog/client/create", "NewsBlogClientApi", "CreateBlog");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/blog/client/update", "NewsBlogClientApi", "UpdateBlog");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/blog/client/post/create", "NewsBlogClientApi", "CreatePost");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/blog/client/post/update", "NewsBlogClientApi", "UpdatePost");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/blog/client/post/{newsId}/delete", "NewsBlogClientApi", "DeletePost");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/blog/client/free/{url}", "NewsBlogClientApi", "BlogUrlIsFree");

            WebApiConfigHelper.ApiRoute(config.Routes, "news/blog/admin/update", "NewsBlogAdminApi", "UpdateBlog");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/blog/{blogId}/delete", "NewsBlogAdminApi", "Delete");

            WebApiConfigHelper.ApiRoute(config.Routes, "news/blog/profile/update", "NewsBlogClientApi", "UpdateProfile");

            WebApiConfigHelper.ApiRoute(config.Routes, "news/tag/pattern/get", "TagApi", "GetTagByPattern");

            WebApiConfigHelper.ApiRoute(config.Routes, "news/settings/get", "NewsSettingsApi", "Get");
            WebApiConfigHelper.ApiRoute(config.Routes, "news/settings/update", "NewsSettingsApi", "Update");
        }
    }
}
