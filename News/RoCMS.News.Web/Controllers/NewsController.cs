using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using System.Linq;
using RoCMS.Base.Models;

namespace RoCMS.News.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsItemService _newsItemService;
        private readonly INewsCategoryService _newsCategoryService;
        private readonly IBlogService _blogService;

        public NewsController(INewsItemService newsItemService, INewsCategoryService newsCategoryService, IBlogService blogService)
        {
            _newsItemService = newsItemService;
            _newsCategoryService = newsCategoryService;
            _blogService = blogService;
        }

        [PagingFilter]
        [AllowAnonymous]
        public ActionResult Page(bool noLayout = false, int pageNumber = 1, int pageSize = 10)
        {
            int totalCount;
            IEnumerable<NewsItem> news = _newsItemService.GetNewsPage(new NewsFilter()
            {
                OnlyPosted = true,
                SortBy = NewsItemSortBy.PostingDate,
                SortOrder = SortOrder.Desc
            }, pageNumber, pageSize, out totalCount);

            ViewBag.TotalCount = totalCount;
            ViewBag.NoLayout = noLayout;
            ViewBag.PagingRoute = typeof(RoCMS.Web.Contract.Models.Page).FullName;
            return PartialView(news);
        }

        [PagingFilter]
        [AllowAnonymous]
        public ActionResult Events(bool future = true, bool noLayout = false, int pageNumber = 1, int pageSize = 10)
        {
            int totalNews;
            IEnumerable<NewsItem> news = _newsItemService.GetNewsPage(new NewsFilter()
                {
                    OnlyPosted = true,
                    OnlyFutureEventDate = future,
                    RecordTypes = new List<RecordType>() {RecordType.Event},
                    SortBy = NewsItemSortBy.EventDate,
                    SortOrder = SortOrder.Asc
                },
                pageNumber, pageSize, out totalNews);

            ViewBag.TotalCount = totalNews;
            ViewBag.NoLayout = noLayout;
            ViewBag.PagingRoute = typeof(RoCMS.Web.Contract.Models.Page).FullName;
            return PartialView("_EventsPage", news);
        }

        [AllowAnonymous]
        public ActionResult News(int id)
        {
            NewsItem news = _newsItemService.GetNewsItem(id);
            return RedirectPermanent(Url.RouteUrl(typeof(RoCMS.News.Contract.Models.NewsItem).FullName, new { relativeUrl = news.RelativeUrl }));
        }

        [MvcSiteMapNode(ParentKey = "Home", Key = "BlogSEF", DynamicNodeProvider = "RoCMS.News.Web.Helpers.NewsDynamicNodeProvider, RoCMS.News.Web")]
        [AllowAnonymous]
        public ActionResult BlogSEF(string relativeUrl)
        {
            try
            {
                string pageUrl = relativeUrl.Split('/').Last();
                NewsItem news = _newsItemService.GetNewsItem(pageUrl, true);

                var requestPath = Request.Path.Substring(1);
                if (!news.CanonicalUrl.Equals(requestPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    // отправляем запрос искать нужный путь. найдётся.
                    return RedirectPermanent(Url.RouteUrl(typeof(NewsItem).FullName, new { relativeUrl = news.RelativeUrl }));
                }

                TempData["MetaKeywords"] = news.MetaKeywords;
                TempData["MetaDescription"] = news.MetaDescription;
                return View("News", news);
            }
            catch (Exception)
            {
                throw new HttpException(404, "Not found");
            }
        }

        //[MvcSiteMapNode(ParentKey = "Home", Key = "NewsCategorySEF", DynamicNodeProvider = "RoCMS.News.Web.Helpers.NewsСategoryDynamicNodeProvider, RoCMS.News.Web")]
        [PagingFilter]
        [AllowAnonymous]
        public ActionResult CategorySEF(string relativeUrl, int pageNumber = 1, int pageSize = 10)
        {
            try
            {

                string pageUrl = relativeUrl.Split('/').Last();
                bool exists = _newsCategoryService.CategoryExists(pageUrl);
                if (!exists)
                {
                    throw new HttpException(404, "Not found");
                }

                var category = _newsCategoryService.GetCategory(pageUrl);

                if (category.CanonicalUrl != relativeUrl)
                {
                    return RedirectPermanent(Url.RouteUrl("BlogModuleSEF", new { relativeUrl = category.CanonicalUrl }));
                }
                ViewBag.CategoryName = category.Name;
                if (category.ChildrenCategories.Any())
                {
                    return View("Subcategories", category);
                }
                else
                {
                    int totalCount;
                    IEnumerable<NewsItem> news =
                        _newsItemService.GetNewsPage(new NewsFilter() {CategoryQuery = category.CategoryId.ToString()},
                            pageNumber, pageSize, out totalCount);
                    ViewBag.TotalCount = totalCount;
                    ViewBag.NoLayout = false;
                    ViewBag.PagingRoute = "CategorySEF";



                    return View("Page", news);
                }
            }
            catch (Exception)
            {
                throw new HttpException(404, "Not found");
            }
        }

        [AllowAnonymous]
        [PagingFilter]
        public ActionResult BlogModuleSEF(string relativeUrl, int pageNumber = 1, int pageSize = 10)
        {
            string pageUrl = relativeUrl.Split('/').Last();
            bool categoryExists = _newsCategoryService.CategoryExists(pageUrl);
            bool newsItemExists = _newsItemService.NewsItemExists(pageUrl);
            if (!categoryExists && !newsItemExists)
            {
                throw new HttpException(404, "Not found");
            }
            var routeValues = Request.RequestContext.RouteData.Values;

            if (categoryExists)
            {
                return CategorySEF(relativeUrl, pageNumber, pageSize);
            }


            return BlogSEF(relativeUrl);

        }

        [AllowAnonymous]
        [Obsolete]
        public ActionResult NewsSEF(string relativeUrl)
        {
            return RedirectToRoutePermanent(typeof(NewsItem).FullName, new { relativeUrl = relativeUrl });
        }
        /// <summary>
        /// Используется при построении страниц аля /blog/tag/блаблабла
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [PagingFilter]
        [AllowAnonymous]
        public ActionResult TagSearch(string tag, int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.Tag = tag;
            int totalCount;
            IEnumerable<NewsItem> news = _newsItemService.GetNewsPage(new NewsFilter()
            {
                TagName = tag,
                OnlyPosted = true,
                SortBy = NewsItemSortBy.PostingDate,
                SortOrder = SortOrder.Desc
            }, pageNumber, pageSize, out totalCount);

            ViewBag.TotalCount = totalCount;
            ViewBag.PagingRoute = "BlogTagSearch";
            return View(news);
        }

        /// <summary>
        /// Используется при встраивании в страницы через виджет
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [PagingFilter]
        [AllowAnonymous]
        public ActionResult Tag(string tag, int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.Tag = tag;
            int totalCount;
            IEnumerable<NewsItem> news = _newsItemService.GetNewsPage(new NewsFilter() {TagName = tag}, pageNumber, pageSize, out totalCount);
            ViewBag.TotalCount = totalCount;
            ViewBag.NoLayout = true;
            ViewBag.PagingRoute = typeof(RoCMS.Web.Contract.Models.Page).FullName;
            return View("Page", news);
        }

        [PagingFilter]
        [AllowAnonymous]
        public ActionResult Rubric(int id, int pageNumber = 1, int pageSize = 10)
        {
            int totalCount;
            ICollection<NewsItem> news = _newsItemService.GetNewsPage(new NewsFilter() {CategoryQuery = id.ToString()},
                pageNumber, pageSize, out totalCount);
            ViewBag.TotalCount = totalCount;
            ViewBag.NoLayout = true;
            ViewBag.PagingRoute = typeof(RoCMS.Web.Contract.Models.Page).FullName;


            return View("Page", news);
        }

        [AllowAnonymous]
        public ActionResult UserBlogItem(string blogUrl, string newsUrl)
        {

            NewsItem news = _newsItemService.GetNewsItem(newsUrl, true);

            TempData["MetaKeywords"] = news.MetaKeywords;
            TempData["MetaDescription"] = news.MetaDescription;

            return View("News", news);
        }


        [AllowAnonymous]
        [PagingFilter]
        public ActionResult UserBlog(string blogUrl, int pageSize, int pageNumber)
        {
            var blog = _blogService.GetBlog(blogUrl);
            int totalCount;
            IEnumerable<NewsItem> news = _newsItemService.GetNewsPage(new NewsFilter() {BlogId = blog.BlogId},
                pageNumber, pageSize, out totalCount);
            ViewBag.TotalCount = totalCount;
            ViewBag.NoLayout = false;
            ViewBag.Header = ViewBag.Title = blog.Title;
            ViewBag.PagingRoute = "UserBlog";
            ViewBag.BlogUrl = blogUrl;
            return View("Page", news);
        }

        [AllowAnonymous]
        public ActionResult TagCloud()
        {
            return View("_TagsCloud");
        }


    }
}
