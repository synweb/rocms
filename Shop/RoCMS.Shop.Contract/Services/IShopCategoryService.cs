using System.Collections.Generic;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopCategoryService
    {
        Category GetCategory(int categoryId);
        Category GetCategory(string relativeUrl);
        int CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int categoryId);
        IList<Category> GetCategories();
        List<Category> GetParentCategoriesWithCurrent(int categoryId);
        void UpdateCategoriesSortOrder(ICollection<Category> categories);
        bool CategoryExists(int id);

        bool CategoryExists(string relativeUrl);
        IList<GoodsItem> GetCategoryGoods(int categoryId, int? count);
        //string GetCategoryCanonicalUrl(int categoryId);
        //string GetCategoryCanonicalUrl(string relativeUrl);
    }
}
