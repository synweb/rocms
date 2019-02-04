using System;
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
            string specs = ParsingHelper.ParseObject<string>(filterContext.HttpContext.Request.QueryString, "specs");
            int? pack = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "pack");
            int? country = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "country");
            int? manufacturer = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "mnf");
            int? minPrice = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "minp");
            int? maxPrice = ParsingHelper.ParseObject<int?>(filterContext.HttpContext.Request.QueryString, "maxp");
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
            filterContext.ActionParameters["specs"] = ParseSpecs(specs);
            filterContext.ActionParameters["minPrice"] = minPrice;
            filterContext.ActionParameters["maxPrice"] = maxPrice;

            base.OnActionExecuting(filterContext);
        }

        private Dictionary<int, string> ParseSpecs(string specsString)
        {
            Dictionary<int, string> specs = new Dictionary<int, string>();
            if (!String.IsNullOrEmpty(specsString))
            {
                //format: 1:150_200,2:_100,5:100_
                var groups = specsString.Split(',');
                foreach (var group in groups)
                {
                    if (string.IsNullOrWhiteSpace(group))
                        continue;
                    try
                    {
                        var parts = group.Split(':');
                        specs.Add(Int32.Parse(parts[0]), parts[1]);
                    }
                    catch
                    {
                    }
                }

            }

            return specs;
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