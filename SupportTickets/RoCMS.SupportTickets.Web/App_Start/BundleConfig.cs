using System.Linq;
using System.Web.Optimization;

namespace RoCMS.SupportTickets.Web
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles, string moduleDir)
        {
            //string adminDir = "~/bin/Content/admin/" + moduleDir;
            //string clientDir = "~/bin/Content/client/" + moduleDir;
            //bundles.Single(x => x.Path == "~/Content/admin/jsbl").IncludeDirectory(adminDir + "/js", "*.js", true);
            //bundles.Single(x => x.Path == "~/Content/admin/cssbl").IncludeDirectory(adminDir + "/css", "*.css", true);

            //bundles.Single(x => x.Path == "~/Content/client/ro/jsbl").IncludeDirectory(clientDir + "/js", "*.js", true);
            //bundles.Single(x => x.Path == "~/Content/client/ro/cssbl").IncludeDirectory(clientDir + "/css", "*.css", true);

            //bundles.Single(x => x.Path == "~/Content/client/ro/jsbl").IncludeDirectory(clientDir + "/js", "*.js", true);
            //bundles.Single(x => x.Path == "~/Content/client/ro/jsbl")
            //    .Include(clientDir + "/js/base.js")
            //    .Include(clientDir + "/js/order.create.js")
            //    .Include(clientDir + "/js/order.filter.js")
            //    .Include(clientDir + "/js/order.list.js")
            //    .Include(clientDir + "/js/register.js");
            //bundles.Single(x => x.Path == "~/Content/client/ro/cssbl").IncludeDirectory(clientDir + "/css", "*.css", true);
            //bundles.Single(x => x.Path == "~/Content/admin/jsbl").IncludeDirectory(adminDir + "/js", "*.js", true);
            //bundles.Single(x => x.Path == "~/Content/admin/cssbl").IncludeDirectory(adminDir + "/css", "*.css", true);
            //bundles.Single(x => x.Path == "~/Content/landing/jsbl").Include(clientDir + "/js/register.js");
        }
    }
}