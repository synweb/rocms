using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RoCMS.Shop.Contract.Services;

namespace Shop.Web
{
    public class RouteConfig
    {

        static RouteConfig()
        {
            var service = DependencyResolver.Current.GetService<IShopSettingsService>();
            try
            {
                var settings = service.GetShopSettings();
                ShopUrl = String.IsNullOrEmpty(settings.ShopUrl) ? "catalog" : settings.ShopUrl;
            }
            catch
            { }
        }

        public static string ShopUrl = "catalog";
        public static void RegisterRoutes(RouteCollection routes)
        {
            IEnumerable<string> controllerNames = typeof (MvcApplication).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .Where(t => !t.IsAbstract)
                .Select(t => String.Format("^{0}$", t.Name.Replace("Controller", "")));

            string constraint = String.Join("|", controllerNames);

            routes.MapRoute(
                name: "DefaultShop",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                constraints: new {controller = constraint}
                );

            routes.MapRoute(
                name: "CatalogSEF",
                url: ShopUrl + "/{*relativeUrl}",
                defaults: new {controller = "Shop", action = "CatalogSEF"},
                constraints: new { relativeUrl = @"\S+" }
                );
        }
    }
}