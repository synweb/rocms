using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Options.Contract;

namespace RoCMS.Options.Web.Controllers
{
    [AuthorizeResources(OptionsRoCmsResources.Options)]
    public class OptionsEditorController : Controller
    {
        //
        // GET: /OptionsEditor/

        [MvcSiteMapNode(Title = "Характеристики", ParentKey = "AdminHome", Key = "Options", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""Options"",""visibility"": ""AdminMenu"", ""iconClass"" : ""fa-list""}")]
        public ActionResult Index()
        {
            return View();
        }



    }
}
