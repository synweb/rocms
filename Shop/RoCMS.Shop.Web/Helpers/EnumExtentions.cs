using System;
using Resources;

namespace RoCMS.Shop.Web.Helpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Возвращает описание значения перечисления из ресурсов, если есть, иначе - значение перечисления
        /// </summary>
        /// <param name="enumValue">значение перечисления</param>
        /// <returns>описание перечисления</returns>
        public static string Description(this Enum enumValue)
        {
            string name = enumValue.ToString();

            string resourceKey = String.Format("{0}_{1}", enumValue.GetType().Name, name);
            string resourceValue = ShopStrings.ResourceManager.GetString(resourceKey);
            return resourceValue ?? name;
        }
    }
}