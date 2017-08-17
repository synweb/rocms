using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using RoCMS.Base.Helpers;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Web.Models.Filters
{
    public class GoodsFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var par = ParamExtractor.ExtractUrlParams(filterContext.HttpContext.Request);

            int? pack = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "pack");
            int? country = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "country");
            int? manufacturer = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "mnf");
            SortCriterion? sort = null;
            if (filterContext.HttpContext.Request.QueryString["sort"] != null)
            {
                sort = ParsingHelper.ParseObject<SortCriterion>(filterContext.HttpContext.Request.QueryString, "sort");
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

        
    }
}