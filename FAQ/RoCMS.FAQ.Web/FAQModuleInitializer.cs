using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RoCMS.Base.Infrastructure;

namespace RoCMS.FAQ.Web
{
    public class FAQModuleInitializer: IModuleInitializer
    {
        private const string MODULE_DIR = "faq";

        public void Init()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles, MODULE_DIR);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            InitBreadcrumbs();
        }

        private void InitBreadcrumbs()
        {
        }
    }
}
