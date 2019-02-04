using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;
using RoCMS.Base.Extentions;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Web.Routing
{
    public class GoodsFilterRoute : RouteBase
    {
        private readonly string _controller;
        private readonly string _action;
        private readonly IDictionary<string, IDictionary<string, string>> _relativeUrlCanonicalUrlDictionary;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeUrlCanonicalUrlDictionary">Словарь, в котором ключ - relativeUrl, а значение - canonicalUrl</param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        public GoodsFilterRoute(IDictionary<string, IDictionary<string, string>> urls, [AspMvcController] string controller, [AspMvcAction] string action)
        {
            if (urls == null)
            {
                throw new ArgumentNullException(nameof(urls));
            }
            if (string.IsNullOrEmpty(controller))
            {
                throw new ArgumentException(nameof(controller));
            }
            if (string.IsNullOrEmpty(action))
            {
                throw new ArgumentException(nameof(action));
            }
            _relativeUrlCanonicalUrlDictionary = urls;
            _controller = controller;
            _action = action;
        }


        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            string url = httpContext.Request.Path;
            if (!url.Contains("/f/") && !url.EndsWith("/f")) return null;

            string filters;
            string canonical;

            if (url.EndsWith("/f"))
            {
                canonical = url.TrimEnd('f').Trim('/');
                filters = null;
            }
            else
            {
                string[] parts = url.Split(new[] {"/f/"}, StringSplitOptions.None);
                filters = parts.Last();
                canonical = parts.First();
            }

            var result = new RouteData(this, new MvcRouteHandler());
            this.AddQueryStringParametersToRouteData(result, httpContext);
            result.Values["controller"] = _controller;
            result.Values["action"] = _action;
            result.Values["relativeUrl"] = canonical;
            result.Values["filters"] = filters;

            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            string url = requestContext.HttpContext.Request.Path;
            if (!url.Contains("/f/") && !url.EndsWith("/f")) return null;

            string relativeUrl = null;
            string filters = null;
            if (values.ContainsKey("relativeUrl"))
            {
                relativeUrl = ((string)values["relativeUrl"]).Trim('/');
            }
            if (values.ContainsKey("filters"))
            {
                filters = (string)values["filters"];
            }



            if (String.IsNullOrEmpty(relativeUrl))
            {
                if (url.EndsWith("/f"))
                {
                    relativeUrl = url.TrimEnd('f').Trim('/');
                    
                }
                else
                {
                    string[] parts = url.Split(new[] { "/f/" }, StringSplitOptions.None);
                    
                    relativeUrl = parts.First();
                }
            }
            if (String.IsNullOrEmpty(filters))
            {
                if (url.EndsWith("/f"))
                {
                    
                    filters = null;
                }
                else
                {
                    string[] parts = url.Split(new[] { "/f/" }, StringSplitOptions.None);
                    filters = parts.Last();
                    
                }
            }



            var otherParams = new Dictionary<string, object>();
            foreach (var param in values)
            {
                if (param.Key != "relativeUrl" && param.Key != "action" && param.Key != "controller" &&
                    param.Key != "area" && param.Key != "filters")
                {
                    otherParams.Add(param.Key, param.Value);
                }
            }

            StringBuilder otherParamsString = new StringBuilder();
            if (otherParams.Any())
            {
                otherParamsString.Append("?");
            }
            foreach (var token in otherParams)
            {
                otherParamsString.Append($"{token.Key}={token.Value}");
                if (token.Key != otherParams.Last().Key)
                {
                    otherParamsString.Append("&");
                }
            }
            var result = new VirtualPathData(this, $"{relativeUrl}/f/{filters}{otherParamsString}");
            return result;
        }

        private void AddQueryStringParametersToRouteData(RouteData routeData, HttpContextBase httpContext)
        {
            var queryString = httpContext.Request.QueryString;
            if (queryString.Keys.Count > 0)
            {
                foreach (var key in queryString.AllKeys)
                {
                    routeData.Values[key] = queryString[key];
                }
            }
        }
    }
}
