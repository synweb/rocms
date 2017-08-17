using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoCMS.Controllers
{
    [AllowAnonymous]
    public class RedirectController : Controller
    {
        public ActionResult Index(string relativeUrl)
        {
            return RedirectPermanent(Url.RouteUrl("PageSEF", new { relativeUrl = relativeUrl }));
        }
    }
}
