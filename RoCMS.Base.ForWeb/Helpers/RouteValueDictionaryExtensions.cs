using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace RoCMS.Base.ForWeb.Helpers
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary RemovePaging(this RouteValueDictionary rv)
        {
            var res = new RouteValueDictionary(rv);
            if (res.ContainsKey("page"))
            {
                res.Remove("page");
            }
            if (res.ContainsKey("pgsize"))
            {
                res.Remove("pgsize");
            }
            return res;
        }

        public static RouteValueDictionary AddRouteParam(this RouteValueDictionary rv, string paramName, int param)
        {
            var res = new RouteValueDictionary(rv);
            if (res.ContainsKey(paramName))
            {
                res[paramName] = @param;
            }
            else
            {
                res.Add(paramName, @param);
            }

            return res;
        }

        public static RouteValueDictionary AddRouteParam(this RouteValueDictionary rv, string paramName, int? param)
        {
            var res = new RouteValueDictionary(rv);
            if (@param.HasValue)
            {
                if (res.ContainsKey(paramName))
                {
                    res[paramName] = @param.Value;
                }
                else
                {
                    res.Add(paramName, @param.Value);
                }
            }
            return res;
        }

        public static RouteValueDictionary AddRouteParam(this RouteValueDictionary rv, string paramName, Enum param)
        {
            string p = param.ToString();
            return rv.AddRouteParam(paramName, p);
        }

        public static RouteValueDictionary AddRouteParam(this RouteValueDictionary rv, string paramName, string param)
        {
            var res = new RouteValueDictionary(rv);
            if (!String.IsNullOrEmpty(@param))
            {
                if (res.ContainsKey(paramName))
                {
                    res[paramName] = @param;
                }
                else
                {
                    res.Add(paramName, @param);
                }
            }
            return res;
        }

        public static RouteValueDictionary RemoveRouteParam(this RouteValueDictionary rv, string paramName)
        {
            var res = new RouteValueDictionary(rv);
            if (res.ContainsKey(paramName))
            {
                res.Remove(paramName);
            }
            return res;
        }

        public static RouteValueDictionary RemoveKeyValueRouteParam(this RouteValueDictionary rv, string paramName,
            string key)
        {
            //вид строки: ключ1:значение1,ключ2:значение2,...
            var res = new RouteValueDictionary(rv);
            
            if (res.ContainsKey(paramName))
            {
                var dict = ((string) res[paramName]).Split(',');
                var resDict = new List<string>(dict);
                foreach (string kvp in dict)
                {
                    var kvp2 = kvp.Split(':');
                    if (kvp2[0] == key)
                    {
                        resDict.Remove(kvp);
                        if (resDict.Count == 0)
                        {
                            res.Remove(paramName);
                        }
                    }
                }
                string keyValueParam = string.Join(",", resDict);
                res[paramName] = keyValueParam;
            }
            return res;
        }

        public static RouteValueDictionary AddKeyValueRouteParam(this RouteValueDictionary rv, string paramName,
            string key, string value)
        {
            string keyValueFormat = string.Format("{0}:{1}", key, value);
            //вид строки: ключ1:значение1,ключ2:значение2,...
            var res = new RouteValueDictionary(rv);
            if (res.ContainsKey(paramName))
            {
                var dict = ((string) res[paramName]).Split(',');
                var resDict = new List<string>(dict);
                foreach (string kvp in dict)
                {
                    var kvp2 = kvp.Split(':');
                    if (kvp2[0] == key)
                    {
                        resDict.Remove(kvp);
                    }
                }
                resDict.Add(keyValueFormat);
                string keyValueParam = string.Join(",", resDict);
                res[paramName] = keyValueParam;
            }
            else
            {
                res.Add(paramName, keyValueFormat);
            }
            return res;
        }
    }
}