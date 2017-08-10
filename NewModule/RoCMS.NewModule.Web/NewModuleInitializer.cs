using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RoCMS.Base.Infrastructure;
using RoCMS.News.Web.App_Start;

namespace RoCMS.News.Web
{
    public class NewModuleInitializer: IModuleInitializer
    {
        private const string MODULE_DIR = "new";

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
            //TODO
        }
    }
}
