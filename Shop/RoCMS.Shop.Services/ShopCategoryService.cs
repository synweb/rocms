using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Shop.Data.Models;
using RoCMS.Web.Contract.Services;
using Category = RoCMS.Shop.Contract.Models.Category;
using GoodsItem = RoCMS.Shop.Contract.Models.GoodsItem;

namespace RoCMS.Shop.Services
{
    class ShopCategoryService : BaseShopService, IShopCategoryService
    {
        private const string CATEGORY_CANNONICAL_URL_CACHE_KEY = "CategoryCannonical:{0}";
        private const string CATEGORY_ID_CANNONICAL_URL_CACHE_KEY = "CategoryCannonicalID:{0}";

        private readonly ISettingsService _settingsService;
        private readonly IShopActionService _shopActionService;
        private readonly CategoryGateway _categoryGateway = new CategoryGateway();
        private readonly SpecCategoryGateway _specCategoryGateway = new SpecCategoryGateway();
        private readonly GoodsCategoryGateway _goodsCategoryGateway = new GoodsCategoryGateway();
        private readonly GoodsItemGateway _goodsItemGateway = new GoodsItemGateway();
        public ShopCategoryService(ISettingsService settingsService, IShopActionService shopActionService)
        {
            InitCache("ShopCategoryService");
            _settingsService = settingsService;
            _shopActionService = shopActionService;
        }

        public Category GetCategory(int categoryId)
        {
            // добавить кэширование
            Data.Models.Category category = _categoryGateway.SelectOne(categoryId);
            var res = Mapper.Map<Category>(category);
            res.CannonicalUrl = GetCategoryCannonicalUrl(category.RelativeUrl);
            FillCanonicalUrls(res.ChildrenCategories);
            return res;
        }

        public Category GetCategory(string relativeUrl)
        {
            // добавить кэширование
            Data.Models.Category category = _categoryGateway.SelectOneByRelativeUrl(relativeUrl);
            var res = Mapper.Map<Category>(category);
            res.CannonicalUrl = GetCategoryCannonicalUrl(category.RelativeUrl);
            FillCanonicalUrls(res.ChildrenCategories);
            return res;
        }

        public int CreateCategory(Category category)
        {
            var dataCategory = Mapper.Map<Data.Models.Category>(category);
            int id = _categoryGateway.Insert(dataCategory);
            RemoveObjectFromCache("Categories");
            return id;
        }

        public void UpdateCategory(Category category)
        {
            var dataRec = Mapper.Map<Data.Models.Category>(category);
            var oldSpecCats = _specCategoryGateway.SelectByCategory(category.CategoryId);

            using (var ts = new TransactionScope())
            {
                _categoryGateway.Update(dataRec);
                foreach (var spec in oldSpecCats.Where(x => category.OrderFormSpecs.All(y => x.SpecId != y.SpecId)).ToList())
                {
                    _specCategoryGateway.Delete(spec);
                }

                foreach (var spec in category.OrderFormSpecs.Where(x => oldSpecCats.All(y => x.SpecId != y.SpecId)))
                {
                    _specCategoryGateway.Insert(new SpecCategory()
                    {
                        CategoryId = category.CategoryId,
                        SpecId = spec.SpecId
                    });
                }
                ts.Complete();
            }
            RemoveObjectFromCache("Categories");
            RemoveObjectFromCache(GetCategoryCannonicalUrlCacheKey(category.RelativeUrl));
            RemoveObjectFromCache(GetCategoryIDCannonicalUrlCacheKey(category.CategoryId));
        }

        public void DeleteCategory(int categoryId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Data.Models.Category dataCategory = _categoryGateway.SelectOne(categoryId);
                var goodsIds = _goodsCategoryGateway.SelectByCategory(categoryId).Select(x => x.HeartId);
                foreach (var goodsId in goodsIds)
                {
                    _goodsItemGateway.Delete(goodsId);
                }
                var childCats = _categoryGateway.Select(categoryId);
                foreach (var child in childCats)
                {
                    _categoryGateway.Delete(child.CategoryId);
                }
                _categoryGateway.Delete(categoryId);
                RemoveObjectFromCache("Categories");
                RemoveObjectFromCache(GetCategoryCannonicalUrlCacheKey(dataCategory.RelativeUrl));
                RemoveObjectFromCache(GetCategoryIDCannonicalUrlCacheKey(dataCategory.CategoryId));

                int? lastCat = _settingsService.GetSettings<int?>("LastGoodsCategory");
                if (lastCat.HasValue && lastCat.Value == categoryId)
                {
                    _settingsService.Set<int?>("LastGoodsCategory", null);
                }
                scope.Complete();
            }
        }

        public IList<Category> GetCategories()
        {
            string cacheKey = "Categories";
            return GetFromCacheOrLoadAndAddToCache(cacheKey, () =>
            {
                var dataCats = _categoryGateway.Select(null);
                var cats = Mapper.Map<List<Category>>(dataCats);
                foreach (var category in cats)
                {
                    FillChildren(category);
                }
                SortCategories(cats);
                FillCannonicalUrls(cats);
                return cats;
            });
        }


        public List<Category> GetParentCategoriesWithCurrent(int categoryId)
        {
            List<Category> result = new List<Category>();
            var category = _categoryGateway.SelectOne(categoryId);
            while (category != null)
            {
                result.Add(Mapper.Map<Category>(category));
                category = category.ParentCategoryId != null ? _categoryGateway.SelectOne(category.ParentCategoryId.Value) : null;
            }
            result.Reverse();
            foreach (var cat in result)
            {
                cat.CannonicalUrl = GetCategoryCannonicalUrl(cat.RelativeUrl);
            }
            return result;
        }

        public void UpdateCategoriesSortOrder(ICollection<Category> categories)
        {
            using (var ts = new TransactionScope())
            {
                int i = 0;
                foreach (var cat in categories)
                {
                    var category = _categoryGateway.SelectOne(cat.CategoryId);
                    category.SortOrder = i;
                    _categoryGateway.Update(category);
                    i++;
                    if (cat.ChildrenCategories.Any())
                    {
                        UpdateCategoriesSortOrder(cat.ChildrenCategories);
                    }
                }
                ts.Complete();
            }
        }

        public bool CategoryExists(int id)
        {
            return _categoryGateway.Exists(id);
        }

        public bool CategoryExists(string relativeUrl)
        {
            return _categoryGateway.ExistsByRelativeUrl(relativeUrl);
        }

        public IList<GoodsItem> GetCategoryGoods(int categoryId, int? count)
        {
            throw new NotImplementedException();
        }

        public string GetCategoryCannonicalUrl(int categoryId)
        {
            string cacheKey = GetCategoryIDCannonicalUrlCacheKey(categoryId);
            return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () =>
            {
                var cat = GetCategory(categoryId);
                return GetCategoryCannonicalUrl(cat.RelativeUrl);
            });
        }
        public string GetCategoryCannonicalUrl(string relativeUrl)
        {
            string cacheKey = GetCategoryCannonicalUrlCacheKey(relativeUrl);
            return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () =>
            {
                string prefix = GetCannonicalCategoryUrlPrefix(relativeUrl);
                return String.IsNullOrEmpty(prefix) ? relativeUrl : String.Format("{0}/{1}", prefix, relativeUrl);
            });
        }

        private void FillCannonicalUrls(ICollection<Category> categories)
        {
            foreach (var category in categories)
            {
                category.CannonicalUrl = GetCategoryCannonicalUrl(category.RelativeUrl);
                if (category.ChildrenCategories.Any())
                {
                    FillCannonicalUrls(category.ChildrenCategories);
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
                child.CannonicalUrl = GetCategoryCannonicalUrl(child.CategoryId);
                child.ParentCategory = new IdNamePair<int>(category.CategoryId, category.Name);
                if (recursive)
                {
                    FillChildren(child);
                }
            }
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
            RemoveObjectFromCache("Categories");
        }

        private string GetCategoryCannonicalUrlCacheKey(string url)
        {
            return String.Format(CATEGORY_CANNONICAL_URL_CACHE_KEY, url);
        }

        private string GetCategoryIDCannonicalUrlCacheKey(int categoryId)
        {
            return String.Format(CATEGORY_ID_CANNONICAL_URL_CACHE_KEY, categoryId);
        }

        private string GetCannonicalCategoryUrlPrefix(string relativeUrl)
        {
            string res = String.Empty;
            bool hasParent = false;
            {
                var rec = _categoryGateway.SelectOneByRelativeUrl(relativeUrl);
                if (rec.ParentCategoryId.HasValue)
                {
                    var parent = _categoryGateway.SelectOne(rec.ParentCategoryId.Value);
                    res = parent.RelativeUrl;
                    hasParent = parent.ParentCategoryId.HasValue;
                }
            }
            if (String.IsNullOrEmpty(res) || !hasParent)
            {
                return res;
            }
            return String.Format("{0}/{1}", GetCannonicalCategoryUrlPrefix(res), res);
        }

        private void FillCanonicalUrls(ICollection<Category> categories)
        {
            foreach (var category in categories)
            {
                category.CannonicalUrl = GetCategoryCannonicalUrl(category.RelativeUrl);
                if (category.ChildrenCategories.Any())
                {
                    FillCanonicalUrls(category.ChildrenCategories);
                }
            }
        }
    }
}
