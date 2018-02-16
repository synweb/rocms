using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Mvc;
using RoCMS.Web.Contract.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Base.ForWeb;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Base.ForWeb.Routing;
using RoCMS.Web.Contract.Infrastructure;

namespace RoCMS
{
    public class RouteConfig
    {
        private static Dictionary<string, string> _redirectUrlsMapping
        {
            get
            {
                try
                {
                    var redirects = new Dictionary<string, string>();
                    RedirectToPageRoutesConfigurationSection redirectSection = (RedirectToPageRoutesConfigurationSection)ConfigurationManager.GetSection("redirectToPageRoutes");
                    foreach (RedirectPageRoute rec in redirectSection.RedirectPageRoutes)
                    {
                        if (string.IsNullOrEmpty(rec.Key))
                            continue;
                        redirects.Add(rec.Key, rec.Value);
                    }                    
                    return redirects;
                }
                catch
                {
                    return new Dictionary<string, string>();
                }
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            XmlSiteMapController.RegisterRoutes(routes);

            //ConfigureSEFRoutes(routes);

            //routes.MapRoute(
            //    name: "PageSEF",
            //    url: "{*relativeUrl}",
            //    defaults: new { controller = "Page", action = "PageSEF" }
            //    );




            routes.MapRoute(
                name: "Thumbnail",
                url: "Gallery/Thumbnail/{id}",
                defaults: new { controller = "Gallery", action = "Thumbnail", id = UrlParameter.Optional });
            routes.MapRoute(
                name: "Image",
                url: "Gallery/Image/{id}",
                defaults: new { controller = "Gallery", action = "Image", id = UrlParameter.Optional });
            routes.MapRoute(
                name: "SetAlbumWatermark",
                url: "api/album/{albumId}/{watermarkImageId}/watermark/set",
                defaults: new { controller = "Gallery", action = "SetAlbumWatermark" });
            routes.MapRoute(
                name: "RemoveAlbumWatermark",
                url: "api/album/{albumId}/watermark/remove",
                defaults: new { controller = "Gallery", action = "RemoveAlbumWatermark" });
            routes.MapRoute(
                name: "FileWithExt",
                url: "File/Get/{fileName}.{ext}",
                defaults: new { controller = "File", action = "Get" });
            routes.MapRoute(
                name: "DeleteFile",
                url: "File/Delete/{fileName}.{ext}",
                defaults: new { controller = "File", action = "DeleteFile" });
            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Home", action = "Login" });
            routes.MapRoute(
                name: "Register",
                url: "register",
                defaults: new { controller = "Home", action = "Register" });

            routes.MapRoute(
                name: "EditPage",
                url: "Admin/EditPage/{relativeUrl}",
                defaults: new { controller = "Admin", action = "EditPage" });
            routes.MapRoute(
                name: "Search",
                url: "search",
                defaults: new { controller = "Home", action = "Search" });
            routes.MapRoute(
                name: "ForgotPassword",
                url: "forgotpassword",
                defaults: new { controller = "Home", action = "ForgotPassword" });
            routes.MapRoute(
                name: "RestorePassword",
                url: "restorepassword/{token}",
                defaults: new { controller = "Home", action = "RestorePassword" });

            IEnumerable<string> controllerNames = typeof(MvcApplication).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller"))
                .Where(t => !t.IsAbstract)
                .Select(t => $"^{t.Name.Replace("Controller", "")}$");
            string constraint = String.Join("|", controllerNames);
            routes.MapRoute(
                name: "DefaultCore",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = constraint }
            );


            RegisterIndexRoute(routes);
            RoutingHelper.RegisterHeartRoute(routes, typeof(Page), "Page", "PageSEF");
            

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }

        private static void RegisterIndexRoute(RouteCollection routes)
        {
            // если в урле ничего нет, то это главная
            var route = new HeartRoute(new List<UrlPair>() {new UrlPair("", "")}, "Page", "MainPage");
            routes.Add("Index", route);
        }

        public static void RegisterRedirects(RouteCollection routes)
        {
            foreach (var redirect in _redirectUrlsMapping)
            {
                routes.MapRoute(
                    name: String.Format("Rdr{0}", redirect.Key),
                    url: redirect.Key,
                    defaults: new { controller = "Redirect", action = "Index", relativeUrl = redirect.Value });
            }
        }
    }
}