﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
            string catFilter = ParsingHelper.ParseObject<string>(filterContext.HttpContext.Request.QueryString, "cats");
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
            filterContext.ActionParameters["catFilter"] = ParseCategoryFilter(catFilter);

            base.OnActionExecuting(filterContext);
        }


        private List<List<int>> ParseCategoryFilter(string catFilter)
        {
            List<List<int>> cats = new List<List<int>>();
            if (catFilter != null)
            {
                // вид:             1,2;3
                // расшифровка:     (1|2)&3
                var orGroups = catFilter.Split(';');
                // сначала разбиваем на группы по "и"
                foreach (var orGroup in orGroups)
                {
                    var intGroup = new List<int>();
                    // формируем группы по "или"
                    intGroup.AddRange(orGroup.Split(',').Select(int.Parse));
                    cats.Add(intGroup);
                }
            }
            return cats;
        }

    }
}