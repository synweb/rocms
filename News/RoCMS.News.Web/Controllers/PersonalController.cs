using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.News.Web.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Web.Controllers
{
    [Authorize]
    public class PersonalController : Controller
    {
        private readonly INewsItemService _newsItemService;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IBlogService _blogService;
        private readonly INewsCategoryService _newsCategoryService;

        public PersonalController(INewsItemService newsItemService, IPrincipalResolver principalResolver, IBlogService blogService, INewsCategoryService newsCategoryService)
        {
            _newsItemService = newsItemService;
            _principalResolver = principalResolver;
            _blogService = blogService;
            _newsCategoryService = newsCategoryService;
        }

        public ActionResult Settings()
        {
            return View();
        }
        public ActionResult CreateBlog()
        {
            int userId = _principalResolver.GetUserId();
            var blog = _blogService.GetUserBlog(userId);
            if (blog != null)
            {
                // если один блог уже есть, больше создавать не даём
                return RedirectToAction("Index");
            }
            var model = new Blog();
            return View("BlogEditor", model);
        }

        public ActionResult EditBlog()
        {
            int userId = _principalResolver.GetUserId();
            var blog = _blogService.GetUserBlog(userId);
            if (blog == null)
            {
                // если блога нет, будем создавать
                return RedirectToAction("CreateBlog");
            }
            return View("BlogEditor", blog);
        }

        public ActionResult Index()
        {
            int userId = _principalResolver.GetUserId();
            var blog = _blogService.GetUserBlog(userId);
            if (blog == null)
            {
                return RedirectToAction("CreateBlog");
            }
            int total;
            ICollection<NewsItem> blogItems = _newsItemService.GetNewsPage(new NewsFilter() {BlogId = blog.BlogId}, 1, int.MaxValue, out total);
            ViewBag.Blog = blog;
            return View(blogItems);
        }

        public ActionResult CreateBlogItem()
        {
            var item = new NewsItem();
            return View("BlogItemEditor", item);
        }

        public ActionResult EditBlogItem(int id)
        {
            ICollection<Category> cats = _newsCategoryService.GetAllCategories();
            ViewBag.Categories = cats;
            int userId = _principalResolver.GetUserId();
            var item = _newsItemService.GetNewsItem(id);
            var blog = _blogService.GetBlog(item.BlogId);
            if (blog.OwnerId != userId)
            {
                throw new UnauthorizedAccessException();
            }

            return View("BlogItemEditor", item);
        }

    }
}
