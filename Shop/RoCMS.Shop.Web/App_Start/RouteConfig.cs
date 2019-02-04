using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Base.ForWeb.Routing;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Web.Routing;
using RoCMS.Web.Contract.Services;
using Action = RoCMS.Shop.Contract.Models.Action;

namespace Shop.Web
{
    public class RouteConfig
    {
        static void RegisterGoodsFilterRoute(RouteCollection routes, [AspMvcController] string controller, [AspMvcAction] string action)
        {
            var heartService = DependencyResolver.Current.GetService<IHeartService>();
            IDictionary<string, IDictionary<string, string>> urls = heartService.GetHeartUrls();


            var route = new GoodsFilterRoute(urls, controller, action);
            routes.Add(typeof(GoodsFilter).FullName, route);
        }

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
            routes.MapRoute(
                name: "Personal",
                url: "personal",
                defaults: new { controller = "Shop", action = "Personal" });

            routes.MapRoute(
                name: "Checkout",
                url: "checkout",
                defaults: new { controller = "Shop", action = "Checkout" });

            routes.MapRoute(
                name: "PickUpPoint",
                url: "pickuppoint/{id}",
                defaults: new { controller = "Shop", action = "PickUpPoint", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "ThankYou",
                url: "thankyou",
                defaults: new { controller = "Shop", action = "ThankYou" });


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

            //routes.MapRoute(
            //    name: "CatalogSEF",
            //    url: ShopUrl + "/{*relativeUrl}",
            //    defaults: new {controller = "Shop", action = "CatalogSEF"},
            //    constraints: new { relativeUrl = @"\S+" }
            //    );

            //routes.MapRoute(
            //    name: typeof(GoodsFilter).FullName,
            //    url: "{*relativeUrl}/f/{filters*}",
            //    defaults: new { controller = "Shop", action = "GoodsFilterSEF" });

            RegisterGoodsFilterRoute(routes, "Shop", "GoodsFilterSEF");

            RoutingHelper.RegisterHeartRoute(routes, typeof(Category), "Shop", "CategorySEF");
            RoutingHelper.RegisterHeartRoute(routes, typeof(GoodsItem), "Shop", "GoodsSEF");
            RoutingHelper.RegisterHeartRoute(routes, typeof(Action), "Shop", "ActionSEF");
            RoutingHelper.RegisterHeartRoute(routes, typeof(Manufacturer), "Shop", "ManufacturerSEF");
        }
    }
}