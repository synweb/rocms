using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoCMS.Models;
using RoCMS.Web.Contract.Models.Shop;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Helpers
{
    public static class BreadCrumbsHelper
    {
        public static List<BreadCrumb> ForSearch()
        {
            var res = new List<BreadCrumb>
            {
                new BreadCrumb()
                {
                    IsLast = true,
                    Title = "Поиск",
                    Url = null
                }
            };
            return res;
        }
        
        public static List<BreadCrumb> ForShopCategory(int categoryId) 
        {
            IShopService shopService = DependencyResolver.Current.GetService<IShopService>();
            IList<Category> categories = shopService.GetParentCategoriesWithCurrent(categoryId);
            List<BreadCrumb> result = new List<BreadCrumb>();
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            foreach (var category in categories)
            {
                result.Add(new BreadCrumb() {
                     Title = category.Name,
                     Url = helper.Action("Category", "Shop", new { id = category.CategoryId }),
                     IsLast = categories.Last() == category
                });
            }
            return result;
        }

        internal static List<BreadCrumb> ForShopGoods(int id)
        {
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            IShopService shopService = DependencyResolver.Current.GetService<IShopService>();
            var goods = shopService.GetGoods(id);
            var result = ForShopCategory(goods.Categories.First().ID);
            result.Last().IsLast = false;
            result.Add(new BreadCrumb() {
                Title = goods.Name,
                Url = helper.Action("Goods", "Shop", new { id = id }),
                IsLast = true
            });
            return result;
        }



        internal static dynamic ForShopAction(int id)
        {
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            List<BreadCrumb> result = new List<BreadCrumb>();
            result.Add(new BreadCrumb()
            {
                Title = "Акции",
                Url = helper.Action("GetPage", "Page", new { relativeUrl = "Акции" }),
                IsLast = false
            });
            IShopService shopService = DependencyResolver.Current.GetService<IShopService>();
            var action = shopService.GetAction(id);
            result.Add(new BreadCrumb()
            {
                Title = action.Name,
                Url = helper.Action("Action", "Shop", new { id = id }),
                IsLast = true
            });
            return result;
        }
    }
}