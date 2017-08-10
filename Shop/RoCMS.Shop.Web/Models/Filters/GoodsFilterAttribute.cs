using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Web.Models.Filters
{
    public class GoodsFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var par = ParamExtractor.ExtractUrlParams(filterContext.HttpContext.Request);

            int? pack = ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "pack");
            int? country = ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "country");
            int? manufacturer = ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "mnf");
            SortCriterion? sort = null;
            if (filterContext.HttpContext.Request.QueryString["sort"] != null)
            {
                sort = ParseObject<SortCriterion>(filterContext.HttpContext.Request.QueryString, "sort");
            }


            filterContext.Controller.ViewBag.PackId = pack;
            filterContext.Controller.ViewBag.CountryId = country;
            filterContext.Controller.ViewBag.Sort = sort;


            filterContext.ActionParameters["packId"] = pack;
            filterContext.ActionParameters["countryId"] = country;
            filterContext.ActionParameters["manufacturerId"] = manufacturer;
            filterContext.ActionParameters["sort"] = sort;

            base.OnActionExecuting(filterContext);
        }



        //КОПИПАСТА ИЗ СКАНА
        /// <summary>
        ///     Парсит GET-строку по заданному ключу и типу возвращаемого объекта
        ///     Прим.: не вызывает исключений (по умолчанию выдает default(T))
        /// </summary>
        /// <typeparam name="T">тип возвращаемого объекта</typeparam>
        /// <param name="dict">GET-словарь</param>
        /// <param name="key">ключ объекта</param>
        static T ParseObject<T>(NameValueCollection dict, string key)
        {
            T defaultValue = default(T);
            string value = dict[key];

            if(String.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            Type type = typeof(T);
            if(type.IsEnum)
            {
                if(Enum.IsDefined(type, value))
                {
                    return (T) Enum.Parse(type, value);
                }

                return defaultValue;
            }
            
            if(type == typeof(Boolean) || type == typeof(Boolean?))
            {
                bool bVal;
                if(!Boolean.TryParse(value, out bVal))
                {
                    int iVal;
                    if(!Int32.TryParse(value, out iVal))
                    {
                        return defaultValue;
                    }

                    return (T)(object)(iVal != 0);
                }

                return (T)(object)bVal;
            }
            if(type == typeof(Int32) || type == typeof(int?))
            {
                int iVal;
                if(!Int32.TryParse(value, out iVal))
                {
                    return defaultValue;
                }

                return (T)(object)iVal;
            }

            if(type == typeof(String))
            {
                return (T)(object)value;
            }

            throw new NotSupportedException();
        }

        
    }
}