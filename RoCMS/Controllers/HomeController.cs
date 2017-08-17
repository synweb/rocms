using System;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Web.Html;
using MvcSiteMapProvider.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Search;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchService _searchService;

        public HomeController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                if (!MvcApplication.IsInstalled)
                    return RedirectToAction("Index", "Install");
                return View("Index");
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Login");
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [AjaxValidateAntiForgeryToken]
        public JsonResult Login(User user, string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            
            if (user != null && !String.IsNullOrWhiteSpace(user.Username) && !String.IsNullOrWhiteSpace(user.Password))
            {
                try
                {
                    int userId = AuthenticationHelper.GetInstance().Login(System.Web.HttpContext.Current, user.Username, user.Password);
                    return Json(new ResultModel(true, new {UserId = userId}));
                }
                catch (AuthenticationException e)
                {
                    return Json(new ResultModel(e));
                }
            
            }
            return Json(new ResultModel(false));
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new RedirectResult("/");
            }
            FormsAuthentication.SignOut();
            var cookie = new System.Web.HttpCookie("userId") {Expires = DateTime.UtcNow.AddYears(-1)};
            System.Web.HttpContext.Current.Response.SetCookie(cookie);
            return new RedirectResult("/");
        }

        [AllowAnonymous]
        public ActionResult LogoutWithoutRedirect()
        {
            Logout();
            return Json(ResultModel.Success);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(User user)
        {
            if (user == null 
                || string.IsNullOrEmpty(user.Username) 
                || string.IsNullOrEmpty(user.Password))
                return null;
            AuthenticationHelper.GetInstance()
                .RegisterUser(System.Web.HttpContext.Current, user.Username, user.Password, user.Email);
            if (!User.Identity.IsAuthenticated)
            {
                AuthenticationHelper.GetInstance().Login(System.Web.HttpContext.Current, user.Username, user.Password);
            }
            if (Request.IsAjaxRequest())
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult OrderCallback()
        {
            return PartialView("OrderCallbackBlock");
        }

        [AllowAnonymous]
        public ActionResult WYSIWYG(string name, string content)
        {
            ViewBag.Name = name;
            return PartialView("WYSIWYG", new Tuple<string, string>(name, content));
        }

        [AllowAnonymous]
        public ActionResult CountdownTimer(string datetime)
        {
            return PartialView(datetime);
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LoginAndRedirect(User user, string ReturnUrl)
        {
            if (string.IsNullOrEmpty(user.Username) 
                || string.IsNullOrEmpty(user.Password))
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl });
            }
            ViewBag.ReturnUrl = ReturnUrl;
            try
            {
                AuthenticationHelper.GetInstance().Login(System.Web.HttpContext.Current, user.Username, user.Password);
                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return new RedirectResult(ReturnUrl);
            }
            catch (AuthenticationException)
            {
                return RedirectToAction("Login", "Home", new { ReturnUrl });
            }
        }

        [AllowAnonymous]
        public ActionResult RestorePassword(Guid token)
        {
            return View(token);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult LoginBlock()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult Sitemap()
        {
            XmlSiteMapProvider x = new XmlSiteMapProvider();
            return new EmptyResult();
        }

        [AllowAnonymous]
        [PagingFilter]
        public ActionResult Search(string query, int page = 1, int pgsize = 10) 
        {
            ViewBag.BreadCrumbs = BreadCrumbsHelper.ForSearch();
            int totalCount;
            int startIndex = (page - 1) * pgsize + 1;
            var results = _searchService.Search(new SearchParams(query), out totalCount, startIndex, pgsize);
            ViewBag.TotalCount = totalCount;
            ViewBag.PagingRoute = "Search";
            return PartialView("SearchResult", results);
        }

        [AuthorizeResources(RoCmsResources.AdminPanel)]
        public ActionResult Restart()
        {
            HttpRuntime.UnloadAppDomain();
            return RedirectToAction("Index", "Admin");
        }
    }
}
