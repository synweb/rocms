using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Base.Helpers;
using RoCMS.Base.Infrastructure;
using RoCMS.Comments.Services;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.Web.Contract.Models.Search;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Web
{
    public class NewsModuleInitializer: IModuleInitializer
    {
        private const string MODULE_DIR = "news";

        public void Init()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterSearch();
            InitBreadcrumbs();
        }

        private void RegisterSearch()
        {
            var searchService = DependencyResolver.Current.GetService<ISearchService>();
            searchService.RegisterRules(typeof (NewsItem), new List<IndexingRule>()
            {
                x =>
                {
                    var item = (NewsItem) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeyTitle,
                        EntityName = x.GetType().FullName,
                        EntityId = item.NewsId.ToString(),
                        SearchContent = SearchHelper.ToSearchIndexText(item.Title),
                        ImageId = item.ImageId,
                        Title = item.Title,
                        Weight = 2,
                        Text = item.Description,
                        Url = RouteConfig.BlogUrl+"/"+item.RelativeUrl
                    };
                },
                x =>
                {
                    var item = (NewsItem) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeyDescription,
                        EntityName = x.GetType().FullName,
                        EntityId = item.NewsId.ToString(),
                        SearchContent = SearchHelper.ToSearchIndexText(item.Description),
                        ImageId = item.ImageId,
                        Title = item.Title,
                        Weight = 1,
                        Text = item.Description,
                        Url = RouteConfig.BlogUrl+"/"+item.RelativeUrl
                    };
                },
                x =>
                {
                    var item = (NewsItem) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeyText,
                        EntityName = x.GetType().FullName,
                        EntityId = item.NewsId.ToString(),
                        SearchContent = SearchHelper.ToSearchIndexText(item.Text),
                        ImageId = item.ImageId,
                        Title = item.Title,
                        Weight = 1,
                        Text = item.Description,
                        Url = RouteConfig.BlogUrl+"/"+item.RelativeUrl
                    };
                }
            });
            searchService.RegisterRules(typeof (Category), new List<IndexingRule>()
            {
                x =>
                {
                    var item = (Category) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeyName,
                        EntityName = x.GetType().FullName,
                        EntityId = item.CategoryId.ToString(),
                        SearchContent = item.Name,
                        Title = item.Name,
                        Text = item.Name,
                        Weight = 1,
                        Url = $"{RouteConfig.BlogUrl}/{item.RelativeUrl}"
                    };
                }
            });
            searchService.RegisterRules(typeof(Blog), new List<IndexingRule>()
            {
                x =>
                {
                    var item = (Blog) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeyRelativeUrl,
                        EntityName = x.GetType().FullName,
                        EntityId = item.BlogId.ToString(),
                        SearchContent = item.RelativeUrl,
                        Title = item.Title,
                        Text = item.RelativeUrl,
                        Weight = 1,
                        Url = $"blogs/{item.RelativeUrl}"
                    };
                },
                                x =>
                {
                    var item = (Blog) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeyTitle,
                        EntityName = x.GetType().FullName,
                        EntityId = item.BlogId.ToString(),
                        SearchContent = item.Title,
                        Title = item.Title,
                        Text = item.Title,
                        Weight = 1,
                        Url = $"blogs/{item.RelativeUrl}"
                    };
                },
                x =>
                {
                    var item = (Blog) x;
                    return new SearchItem()
                    {
                        SearchItemKey = item.SearchKeySubtitle,
                        EntityName = x.GetType().FullName,
                        EntityId = item.BlogId.ToString(),
                        SearchContent = item.Subtitle,
                        Title = item.Title,
                        Text = item.Subtitle,
                        Weight = 1,
                        Url = $"blogs/{item.RelativeUrl}"
                    };
                }
            });
        }

        private void InitBreadcrumbs()
        {
            BreadCrumbsHelper.RegisterPattern(new BreadCrumbPattern("^/"+ RouteConfig.BlogUrl + "/tag/[^/]+$", url =>
            {
                var res = new List<BreadCrumb>();
                var pageService = DependencyResolver.Current.GetService<IPageService>();
                var blogTitle = pageService.GetPage(RouteConfig.BlogUrl).Title;
                res.Add(new BreadCrumb()
                {
                    IsLast = false,
                    Title = blogTitle,
                    Url = "/" + RouteConfig.BlogUrl
                });

                var tag = Regex.Match(url, "^/" + RouteConfig.BlogUrl + "/tag/([^/]+)$").Groups[1].Value;
                res.Add(new BreadCrumb()
                {
                    IsLast = true,
                    Title = "Записи с тегом «" + tag + "»",
                    Url = url
                });
                return res;
            }));

            BreadCrumbsHelper.RegisterPattern(new BreadCrumbPattern("^/" + RouteConfig.BlogUrl + "/[^/]+$", url =>
            {
                var res = new List<BreadCrumb>();
                var pageService = DependencyResolver.Current.GetService<IPageService>();
                var blogTitle = pageService.GetPage(RouteConfig.BlogUrl).Title;
                res.Add(new BreadCrumb()
                {
                    IsLast = false,
                    Title = blogTitle,
                    Url = "/" + RouteConfig.BlogUrl
                });

                var relativeUrl = Regex.Match(url, "^/" + RouteConfig.BlogUrl + "/(.+)$").Groups[1].Value;
                var newsService = DependencyResolver.Current.GetService<INewsItemService>();
                var title = newsService.GetNewsItem(relativeUrl, false).Title;
                res.Add(new BreadCrumb()
                {
                    IsLast = true,
                    Title = title,
                    Url = url
                });
                return res;
            }));


        }
    }
}
