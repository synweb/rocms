using System;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;
using RoCMS.Base.ForWeb.Routing;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Base.ForWeb.Helpers
{
    public static class RoutingHelper
    {
        public static void RegisterHeartRoute(RouteCollection routes, Type type, [AspMvcController] string controller, [AspMvcAction] string action)
        {
            var heartService = DependencyResolver.Current.GetService<IHeartService>();
            var urls = heartService.GetHeartUrls(type);
            var route = new HeartRoute(urls, controller, action);
            routes.Add(type.FullName, route);
        }
    }
}
