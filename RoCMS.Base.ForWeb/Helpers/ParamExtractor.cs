using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoCMS.Base.ForWeb.Helpers
{
    public static class ParamExtractor
    {
        /// <summary>
        /// Вытащить из запроса все параметры, находящиеся в адресной строке
        /// </summary>
        public static IDictionary<string, object> ExtractUrlParams(HttpRequestBase request)
        {
            var @params = request.QueryString.AllKeys;
            var res = new Dictionary<string, object>();
            foreach (var param in @params)
            {
                var val = request.QueryString[param];
                res.Add(param, val);
            }
            return res;
        }

        /// <summary>
        /// Вытащить из запроса параметры и адресной строки, и маршрута
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ExtractAllParams(HttpRequestBase request)
        {
            var res = ExtractUrlParams(request);
            foreach (var kvp in request.RequestContext.RouteData.Values)
            {
                if(res.ContainsKey(kvp.Key)) continue;
                res.Add(kvp);
            }
            return res;
        }

        /// <summary>
        /// Вытащить из запроса параметры для передачи в человекочитаемый URL. Исключает параметры, определяющие маршрут.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ExtractParamsForSEF(HttpRequestBase request)
        {
            var res = ExtractAllParams(request).Where(x => !(x.Key == "action" || x.Key == "controller"));
            return res.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}