using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RoCMS.Models;

namespace RoCMS.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Index(HttpStatusCode code, Guid? errorID)
        {
            Response.StatusCode = Convert.ToInt32(code);
            var model = new ErrorModel(code, errorID);
            switch (code)
            {
                case HttpStatusCode.NotFound:
                    return View("NotFound", model);
                default:
                    return View(new ErrorModel(code, errorID));
            }
            
        }
    }
}
