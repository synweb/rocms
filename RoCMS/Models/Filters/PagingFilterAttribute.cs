using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoCMS.Models.Filters
{
    public class PagingFilterAttribute : ActionFilterAttribute
    {


        #region Methods

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int pageSize;
            int page;
            bool allPages = ParseObject<string>(filterContext.HttpContext.Request.QueryString, "pgsize") == "all";

            if (allPages)
            {
                page = 1;
                pageSize = int.MaxValue;
            }
            else
            {
                page = ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "page") ?? 1;
                pageSize = ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "pgsize") ?? 10;
            }
            filterContext.Controller.ViewBag.Page = page;
            filterContext.Controller.ViewBag.PageSize = pageSize;

            //filterContext.ActionParameters["pageSize"] = (int)pageSize;
            //filterContext.ActionParameters["pageNumber"] = page;

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
                int intValue;
                if(Int32.TryParse(value, out intValue))
                {
                    return (T)Enum.ToObject(type, intValue);
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

        #endregion
    }
}