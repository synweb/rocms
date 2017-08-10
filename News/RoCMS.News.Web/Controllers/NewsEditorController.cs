using System.Collections.Generic;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.News.Web.Models;
using MvcSiteMapProvider;

namespace RoCMS.News.Web.Controllers
{
    [AuthorizeResources(RoCmsResources.News)]
    public class NewsEditorController : Controller
    {
        private readonly INewsItemService _newsItemService;
        private readonly IBlogService _blogService;

        public NewsEditorController(INewsItemService newsItemService, IBlogService blogService)
        {
            _newsItemService = newsItemService;
            _blogService = blogService;
        }

        [MvcSiteMapNode(Title = "Новости", ParentKey = "AdminHome", Key = "News", Clickable = false, VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""News"", ""visibility"": ""AdminMenu"", ""iconClass"" : ""fa-flash"" }")]
        public void Index()
        {

        }
        
        [MvcSiteMapNode(Title = "Новая запись", ParentKey = "News", Key = "CreateNewsItem", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""News"", ""visibility"": ""AdminMenu"", ""iconClass"" : ""fa-pencil-square-o""}")]
        public ActionResult CreateNews()
        {
            ViewBag.Action = "Create";
            return PartialView("NewsEditor");
        }

        //[MvcSiteMapNode(Title = "Все записи", ParentKey = "News", Key = "NewsItems", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""News"", ""visibility"": ""AdminMenu""}")]
        [PagingFilter]
        public ActionResult News(int pageNumber = 1, int pageSize = 10)
        {
            int totalCount;
            int blogId = 1;//админский блог всегда первый


            IEnumerable<NewsItem> news =
                _newsItemService.GetNewsPage(
                    new NewsFilter() { OnlyPosted = false, SortBy = NewsItemSortBy.CreationDate, BlogId = blogId}, pageNumber, pageSize,
                    out totalCount);
            NewsVM vm = new NewsVM(news, totalCount, pageNumber, pageSize);
            ViewBag.Header = "Лента администрации";
            return PartialView(vm);
        }

        [MvcSiteMapNode(Title = "Все записи", ParentKey = "News", Key = "NewsItems", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""News"", ""visibility"": ""AdminMenu""}")]
        [PagingFilter]
        public ActionResult AllUserNews(int pageNumber = 1, int pageSize = 10)
        {
            int totalCount;

            IEnumerable<NewsItem> news =
                _newsItemService.GetNewsPage(
                    new NewsFilter() {OnlyPosted = false, SortBy = NewsItemSortBy.CreationDate}, pageNumber, pageSize,
                    out totalCount);
            NewsVM vm = new NewsVM(news, totalCount, pageNumber, pageSize);
            ViewBag.Header = "Все новости";

            return PartialView("News", vm);
        }

        [MvcSiteMapNode(Title = "Рубрики", ParentKey = "News", Key = "Categories", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""News"", ""visibility"": ""AdminMenu""}")]
        public ActionResult Categories()
        {
            return PartialView();
        }

        [MvcSiteMapNode(Title = "Настройки", ParentKey = "News", Key = "Settings", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""News"", ""visibility"": ""AdminMenu""}")]
        public ActionResult Settings()
        {
            return PartialView();
        }

        public ActionResult EditNews(int id)
        {
            ViewBag.Action = "Edit";
            NewsItem item = _newsItemService.GetNewsItem(id);
            return PartialView("NewsEditor", item);
        }

        [MvcSiteMapNode(Title = "Блоги", ParentKey = "News", Key = "Blogs", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""News"", ""visibility"": ""AdminMenu""}")]
        public ActionResult Blogs()
        {


            var blogs = _blogService.GetBlogs();

            return View(blogs);
        }

        public ActionResult EditBlog(int id)
        {
            var blog = _blogService.GetBlog(id);
            return View("BlogEditor", blog);
        }
    }
}
