using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using JetBrains.Annotations;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Helpers;

namespace RoCMS.Options.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            WebApiConfigHelper.ApiRoute(config.Routes, "options/get", "OptionsApi", "GetOptions");
            WebApiConfigHelper.ApiRoute(config.Routes, "options/{id}/get", "OptionsApi", "Get");
            WebApiConfigHelper.ApiRoute(config.Routes, "options/{id}/delete", "OptionsApi", "Remove");
            WebApiConfigHelper.ApiRoute(config.Routes, "options/create", "OptionsApi", "Create");
            WebApiConfigHelper.ApiRoute(config.Routes, "options/update", "OptionsApi", "Update");
        }
    }
}
