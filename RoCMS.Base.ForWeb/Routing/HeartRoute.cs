using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;
using RoCMS.Base.ForWeb.Helpers;
using RoCMS.Web.Contract.Infrastructure;

namespace RoCMS.Base.ForWeb.Routing
{
    public class HeartRoute: RouteBase
    {
        private readonly string _controller;
        private readonly string _action;
        private readonly ICollection<UrlPair> _urls;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urls">Словарь, в котором ключ - relativeUrl, а значение - canonicalUrl</param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        public HeartRoute(ICollection<UrlPair> urls, [AspMvcController] string controller, [AspMvcAction] string action)
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
            _urls = urls;
            _controller = controller;
            _action = action;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var relativeUrl = httpContext.Request.Path.Split('/').Last();
            var urlPair = _urls.FirstOrDefault(x => x.RelativeUrl.Equals(relativeUrl, StringComparison.InvariantCultureIgnoreCase));
            if (urlPair == null)
                return null;
            var result = new RouteData(this, new MvcRouteHandler());
            this.AddQueryStringParametersToRouteData(result, httpContext);
            result.Values["controller"] = _controller;
            result.Values["action"] = _action;
            result.Values["relativeUrl"] = urlPair.RelativeUrl;
            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {

            string relativeUrl;
            if (values.ContainsKey("relativeUrl"))
            {
                relativeUrl = (string) values["relativeUrl"];
            }
            else
            {
                relativeUrl = requestContext.HttpContext.Request.Path;
            }

            relativeUrl = relativeUrl.Split('/').Last().ToLower();

            var urlPair =
                _urls.FirstOrDefault(x => x.RelativeUrl.Equals(relativeUrl, StringComparison.InvariantCultureIgnoreCase));
            if (urlPair == null)
                return null;

            var otherParams = new Dictionary<string, object>();
            foreach (var param in values)
            {
                if (param.Key != "relativeUrl" && param.Key != "action" && param.Key != "controller" &&
                    param.Key != "area")
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

            var result = new VirtualPathData(this, $"{urlPair.CanonicalUrl}{otherParamsString}");

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
