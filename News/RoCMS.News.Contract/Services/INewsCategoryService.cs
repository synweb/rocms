using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.News.Contract.Models;

namespace RoCMS.News.Contract.Services
{
    public interface INewsCategoryService
    {
        Category GetCategory(int categoryId);
        Category GetCategory(string relativeUrl);
        //Category GetCategory(string relativeUrl);
        int CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int categoryId);
        ICollection<Category> GetCategories();
        ICollection<Category> GetAllCategories();
        ICollection<Category> GetParentCategoriesWithCurrent(int categoryId);
        void UpdateCategoriesSortOrder(ICollection<Category> categories);
        bool CategoryExists(int id);
        bool CategoryExists(string relativeUrl);
        string GetCategoryCanonicalUrl(int categoryId);
        
    }
}
