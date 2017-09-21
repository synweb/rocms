using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Helpers;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly ISettingsService _settingsService;

        public PageController(IPageService pageService, ISettingsService settingsService)
        {
            _pageService = pageService;
            _settingsService = settingsService;
        }

        [AllowAnonymous]
        public ActionResult MainPage()
        {
            string homepageUrl = _settingsService.GetHomepageUrl();
            var page = _pageService.GetPage(homepageUrl);
            if (page == null)
                throw new HttpException(404, "Not found");
            TempData["MetaKeywords"] = page.MetaKeywords;
            TempData["MetaDescription"] = page.MetaDescription;
            TempData["AdditionalHeaders"] = page.AdditionalHeaders;
            return PartialView("Index", page);
        }

        [AllowAnonymous]
        public ActionResult GetPage(string relativeUrl)
        {
            return RedirectPermanent(Url.RouteUrl("PageSEF", new {relativeUrl = relativeUrl}));
        }

        [MvcSiteMapNode(ParentKey = "Home", Key = "PageSEF", DynamicNodeProvider = "RoCMS.Helpers.PageDynamicNodeProvider, RoCMS")]
        [AllowAnonymous]
        public ActionResult PageSEF(string relativeUrl)
        {
            try
            {
                string pageUrl = relativeUrl.Split('/').Last();
                string homepage = _settingsService.GetHomepageUrl();
                if (pageUrl == homepage)
                {
                    return RedirectPermanent("/");
                }
                var page = _pageService.GetPage(pageUrl);
                if (page.CannonicalUrl != relativeUrl)
                {
                    return RedirectPermanent(Url.RouteUrl("PageSEF", new { relativeUrl = page.CannonicalUrl }));
                }
                TempData["MetaKeywords"] = page.MetaKeywords;
                TempData["MetaDescription"] = page.MetaDescription;
                TempData["AdditionalHeaders"] = page.AdditionalHeaders;
                return View("Index", page);
            }
            catch (Exception)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                // TODO: это не совсем гуд, так как все 404 в итоге спамятся в лог
                throw new HttpException(404, "Not found");
            }
        }

    }
}
