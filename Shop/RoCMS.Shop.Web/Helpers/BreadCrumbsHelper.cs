using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.Helpers
{
    public static class BreadCrumbsHelper
    {
        
        public static List<BreadCrumb> ForShopCategory(int categoryId) 
        {
            IShopCategoryService shopCategoryService = DependencyResolver.Current.GetService<IShopCategoryService>();
            IList<Category> categories = shopCategoryService.GetParentCategoriesWithCurrent(categoryId);
            List<BreadCrumb> result = new List<BreadCrumb>();
            UrlHelper helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            foreach (var category in categories)
            {
                result.Add(new BreadCrumb() {
                     Title = category.Name,
                     Url = helper.RouteUrl("CatalogSEF", new {relativeURL = category.CannonicalUrl}),
                     IsLast = categories.Last() == category
                });
            }
            return result;
        }

        public static List<BreadCrumb> ForShopGoods(int id)
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
            IShopActionService shopActionService = DependencyResolver.Current.GetService<IShopActionService>();
            var action = shopActionService.GetAction(id);
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