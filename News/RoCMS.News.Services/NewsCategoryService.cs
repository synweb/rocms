using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Base.Models;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.News.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Services
{
    public class NewsCategoryService: NewsService, INewsCategoryService
    {
        private const string CATEGORY_ID_CANNONICAL_URL_CACHE_KEY = "CategoryCannonicalID:{0}";
        public NewsCategoryService(ISearchService searchService)
        {
            _searchService = searchService;
            InitCache("NewsCategoryService");
        }

        private readonly CategoryGateway _categoryGateway = new CategoryGateway();
        private readonly ISearchService _searchService;

        public Category GetCategory(int categoryId)
        {
            var dataRes = _categoryGateway.SelectOne(categoryId);
            var res = Mapper.Map<Category>(dataRes);
            res.CanonicalUrl = GetCategoryCannonicalUrl(res.CategoryId);
            FillParent(res);
            FillChildren(res, false);
            //TODO: скоро здесь будет кэширование :)
            return res;
        }

        private void FillParent(Category res)
        {
            if(res.ParentCategoryId == null)
                return;
            var parent = _categoryGateway.SelectOne(res.ParentCategoryId.Value);
            res.ParentCategory = new IdNamePair<int>(parent.CategoryId, parent.Name);
        }

        public int CreateCategory(Category category)
        {
            var dataRec = Mapper.Map<Data.Models.Category>(category);
            int id = _categoryGateway.Insert(dataRec);
            //RemoveObjectFromCache("Categories");
            category.CategoryId = id;
            _searchService.UpdateIndex(category);
            return id;
        }

        public void UpdateCategory(Category category)
        {
            var dataRec = Mapper.Map<Data.Models.Category>(category);
            _categoryGateway.Update(dataRec);
            //RemoveObjectFromCache("Categories");
            _searchService.UpdateIndex(category);
            RemoveObjectFromCache(GetCategoryIDCannonicalUrlCacheKey(category.CategoryId));
        }

        public void DeleteCategory(int categoryId)
        {
            _categoryGateway.Delete(categoryId);
            _searchService.RemoveFromIndex(typeof(Category), categoryId);
            //RemoveObjectFromCache("Categories");
        }

        public ICollection<Category> GetCategories()
        {
            var root = _categoryGateway.Select();
            var resRoot = Mapper.Map<List<Category>>(root);
            foreach (var category in resRoot)
            {
                FillChildren(category);
                category.CanonicalUrl = GetCategoryCannonicalUrl(category.CategoryId);
            }
            SortCategories(resRoot);
            return resRoot;
        }

        public ICollection<Category> GetAllCategories()
        {
            var root = _categoryGateway.SelectAll();
            var resRoot = Mapper.Map<List<Category>>(root);
            foreach (var category in resRoot)
            {
                category.CanonicalUrl = GetCategoryCannonicalUrl(category.CategoryId);
            }
            return resRoot;
        }

        private void SortCategories(List<Category> categories)
        {
            categories.Sort((x1, x2) => x1.SortOrder.CompareTo(x2.SortOrder));
            foreach (var cat in categories)
            {
                if (cat.ChildrenCategories.Any())
                {
                    var cats = cat.ChildrenCategories.ToList();
                    SortCategories(cats);
                    cat.ChildrenCategories = cats;
                }
            }
        }

        private void FillChildren(Category category, bool recursive = true)
        {
            var children = _categoryGateway.Select(category.CategoryId);
            var resChildren = Mapper.Map<ICollection<Category>>(children);
            category.ChildrenCategories = resChildren;
            foreach (var child in resChildren)
            {
                child.CanonicalUrl = GetCategoryCannonicalUrl(child.CategoryId);
                child.ParentCategory = new IdNamePair<int>(category.CategoryId, category.Name);
                if (recursive)
                {
                    FillChildren(child);
                }
            }
        }

        public ICollection<Category> GetParentCategoriesWithCurrent(int categoryId)
        {
            List<Category> result = new List<Category>();
            var category = _categoryGateway.SelectOne(categoryId);
            result.Add(Mapper.Map<Category>(category));
            while (category.ParentCategoryId != null)
            {
                category = _categoryGateway.SelectOne(category.ParentCategoryId.Value);
                result.Add(Mapper.Map<Category>(category));
            }
            result.Reverse();
            return result;
        }

        public void UpdateCategoriesSortOrder(ICollection<Category> categories)
        {
            RemoveObjectFromCache("Categories");
            using (var ts = new TransactionScope())
            {
                UpdateCategoriesSortOrderInternal(categories);
                ts.Complete();
            }
        }

        private void UpdateCategoriesSortOrderInternal(ICollection<Category> categories)
        {
            int i = 0;
            foreach (var cat in categories)
            {
                //Можно сделать быстрее, если не вынимать всю категорию
                var category = _categoryGateway.SelectOne(cat.CategoryId);
                category.SortOrder = i;
                _categoryGateway.Update(category);
                i++;
                if (cat.ChildrenCategories.Any())
                {
                    UpdateCategoriesSortOrderInternal(cat.ChildrenCategories);
                }
            }
        }

        public bool CategoryExists(int id)
        {
            var cat = _categoryGateway.SelectOne(id);
            return cat != null;
        }

        public bool CategoryExists(string relativeUrl)
        {
            var cat = _categoryGateway.SelectByUrl(relativeUrl);
            return cat != null;
        }

        public string GetCategoryCannonicalUrl(int categoryId)
        {

            string cacheKey = GetCategoryIDCannonicalUrlCacheKey(categoryId);
            return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () =>
            {

                var cat = _categoryGateway.SelectOne(categoryId);
                string result;
                if (cat.ParentCategoryId.HasValue)
                {
                    result = $"{GetCategoryCannonicalUrl(cat.ParentCategoryId.Value)}/{cat.RelativeUrl}";
                }
                else
                {
                    result = cat.RelativeUrl;
                }
                return result;
            });
        }

        private string GetCategoryIDCannonicalUrlCacheKey(int categoryId)
        {
            return string.Format(CATEGORY_ID_CANNONICAL_URL_CACHE_KEY, categoryId);
        }

        public Category GetCategory(string relativeUrl)
        {
            var dataRes = _categoryGateway.SelectByUrl(relativeUrl);
            var res = Mapper.Map<Category>(dataRes);
            res.CanonicalUrl = GetCategoryCannonicalUrl(res.CategoryId);
            FillParent(res);
            FillChildren(res, false);
            //TODO: скоро здесь будет кэширование :)
            return res;
        }
    }
}
