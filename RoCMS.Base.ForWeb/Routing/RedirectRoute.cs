using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RoCMS.Base.ForWeb.Routing
{
    public class RedirectRoute: RouteBase
    {
        private readonly IDictionary<string, string> _redirectsDictionary;

        public RedirectRoute(IDictionary<string, string> redirectsDictionary)
        {
            _redirectsDictionary = redirectsDictionary;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var url = httpContext.Request.Path.ToLower();
            if (!_redirectsDictionary.ContainsKey(url))
                return null;
            var targetUrl = _redirectsDictionary[url];
            var result = new RouteData(this, new MvcRouteHandler());
            this.AddQueryStringParametersToRouteData(result, httpContext);
            result.Values["controller"] = "Redirect";
            result.Values["action"] = "Index";
            result.Values["relativeUrl"] = targetUrl;
            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            string url = requestContext.HttpContext.Request.Path.ToLower();
            if (!_redirectsDictionary.ContainsKey(url))
            {
                return null;
            }
            StringBuilder otherParamsString = new StringBuilder();
            var otherParams = new Dictionary<string, object>();
            foreach (var param in values)
            {
                if (param.Key != "relativeUrl" && param.Key != "action" && param.Key != "controller" &&
                    param.Key != "area")
                {
                    otherParams.Add(param.Key, param.Value);
                }
            }
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
            var targetUrl = _redirectsDictionary[url];
            var result = new VirtualPathData(this, $"{targetUrl}{otherParamsString}");
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
