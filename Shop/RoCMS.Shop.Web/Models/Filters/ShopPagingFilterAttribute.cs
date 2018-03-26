using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider.DI;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.Models.Filters
{
    public class ShopPagingFilterAttribute : PagingFilterAttribute
    {
        private static int shopDefaultPageSize = 10;

        static ShopPagingFilterAttribute()
        {
            var settingsService = DependencyResolver.Current.GetService<IShopSettingsService>();
            var settings = settingsService.GetShopSettings();
            if (settings.DefaultPageSize > 0)
            {
                shopDefaultPageSize = settings.DefaultPageSize;
            }
        }

        public ShopPagingFilterAttribute()
        {
            _defaultPageSize = shopDefaultPageSize;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}