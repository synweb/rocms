using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Shop.Contract
{
    public static class ShopAppSettingsHelper
    {
        /// <summary>
        /// Категории, из которых набираются бестселлеры, пока магазин ничего не продал
        /// </summary>
        public static int[] RecommendedCategories
        {
            get
            {
                var strings = ConfigurationManager.AppSettings.Get("RecommendedCategories").Split(',');
                return strings.Select(int.Parse).ToArray();
            }
        }
    }
}
