using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RoCMS.Base.Infrastructure;
using RoCMS.Options.Web;

namespace RoCMS.News.Web
{
    public class NewsModuleInitializer: IModuleInitializer
    {
        private const string MODULE_DIR = "options";

        public void Init()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles, MODULE_DIR);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

    }
}