﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
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
        private readonly IHeartService _heartService;

        private readonly CategoryGateway _categoryGateway = new CategoryGateway();
        private readonly SpecCategoryGateway _specCategoryGateway = new SpecCategoryGateway();
        private readonly GoodsCategoryGateway _goodsCategoryGateway = new GoodsCategoryGateway();
        private readonly GoodsItemGateway _goodsItemGateway = new GoodsItemGateway();
        public ShopCategoryService(ISettingsService settingsService, IShopActionService shopActionService, IHeartService heartService)
        {
            InitCache("ShopCategoryService");
            _settingsService = settingsService;
            _shopActionService = shopActionService;
            _heartService = heartService;
        }

        public Category GetCategory(int categoryId)
        {
            // добавить кэширование
            Data.Models.Category category = _categoryGateway.SelectOne(categoryId);
            var res = Mapper.Map<Category>(category);

            FillData(res);
            return res;
        }

        public Category GetCategory(string relativeUrl)
        {
            // добавить кэширование
            Data.Models.Category category = _categoryGateway.SelectOneByRelativeUrl(relativeUrl);
            var res = Mapper.Map<Category>(category);
            FillData(res);

            return res;
        }

        public int CreateCategory(Category category)
        {

            category.Type = category.GetType().FullName;

            var dataCategory = Mapper.Map<Data.Models.Category>(category);

            using(var ts = new TransactionScope())
            {
                int id = category.HeartId = dataCategory.HeartId = _heartService.CreateHeart(category);
                _categoryGateway.Insert(dataCategory);
                RemoveObjectFromCache("Categories");
                RemoveObjectFromCache("AllCategories");
                RemoveObjectFromCache($"ChildCategoriesFor{category.ParentCategoryId}");
                ts.Complete();
                return id;
            }

        }

        public void UpdateCategory(Category category)
        {
            var oldCategory = GetCategory(category.HeartId);

            var dataRec = Mapper.Map<Data.Models.Category>(category);
            var oldSpecCats = _specCategoryGateway.SelectByCategory(category.HeartId);

            using (var ts = new TransactionScope())
            {

                _heartService.UpdateHeart(category);

                _categoryGateway.Update(dataRec);
                foreach (var spec in oldSpecCats.Where(x => category.OrderFormSpecs.All(y => x.SpecId != y.SpecId)).ToList())
                {
                    _specCategoryGateway.Delete(spec);
                }

                foreach (var spec in category.OrderFormSpecs.Where(x => oldSpecCats.All(y => x.SpecId != y.SpecId)))
                {
                    _specCategoryGateway.Insert(new SpecCategory()
                    {
                        CategoryId = category.HeartId,
                        SpecId = spec.SpecId
                    });
                }
                ts.Complete();
            }


            RemoveObjectFromCache($"ChildCategoriesFor{oldCategory.ParentCategoryId}");
            RemoveObjectFromCache($"ChildCategoriesFor{category.ParentCategoryId}");
            

            RemoveObjectFromCache("Categories");
            RemoveObjectFromCache("AllCategories");
            RemoveObjectFromCache(GetCategoryCanonicalUrlCacheKey(category.RelativeUrl));
            RemoveObjectFromCache(GetCategoryCanonicalUrlCacheKey(oldCategory.RelativeUrl));

            RemoveObjectFromCache(GetCategoryIDCanonicalUrlCacheKey(category.HeartId));
        }

        public void DeleteCategory(int heartId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Delete(heartId);
                scope.Complete();
            }
        }

        private void Delete(int heartId)
        {

            var shopService = DependencyResolver.Current.GetService<IShopService>();

            Data.Models.Category dataCategory = _categoryGateway.SelectOne(heartId);
            var goodsIds = _goodsCategoryGateway.SelectByCategory(heartId).Select(x => x.GoodsId);
            foreach (var goodsId in goodsIds)
            {
                var goodscats = _goodsCategoryGateway.SelectByGoods(goodsId);
                if (goodscats.Count() == 1)
                {
                    shopService.DeleteGoods(goodsId);
                }
            }

            var childCats = _categoryGateway.Select(heartId);
            foreach (var child in childCats)
            {
                Delete(child.HeartId);
            }

            _categoryGateway.Delete(heartId);
            var heart = _heartService.GetHeart(heartId);
            _heartService.DeleteHeart(heartId);


            RemoveObjectFromCache("Categories");
            RemoveObjectFromCache("AllCategories");
            RemoveObjectFromCache($"ChildCategoriesFor{dataCategory.ParentCategoryId}");

            RemoveObjectFromCache(GetCategoryCanonicalUrlCacheKey(heart.RelativeUrl));
            RemoveObjectFromCache(GetCategoryIDCanonicalUrlCacheKey(dataCategory.HeartId));

            int? lastCat = _settingsService.GetSettings<int?>("LastGoodsCategory");
            if (lastCat.HasValue && lastCat.Value == heartId)
            {
                _settingsService.Set<int?>("LastGoodsCategory", null);
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
                    FillData(category);
                }
                
                SortCategories(cats);

                return cats;
            });
        }

        public IList<Category> GetAllCategories()
        {
            string cacheKey = "AllCategories";
            return GetFromCacheOrLoadAndAddToCache(cacheKey, () =>
            {
                var dataCats = _categoryGateway.SelectAll();
                var cats = Mapper.Map<List<Category>>(dataCats);
                foreach (var category in cats)
                {
                    FillData(category);
                }
                return cats;
            });
        }


        public List<Category> GetParentCategoriesWithCurrent(int categoryId)
        {
            List<Category> result = new List<Category>();
            var category = _categoryGateway.SelectOne(categoryId);
            while (category != null)
            {
                var cat = Mapper.Map<Category>(category);
                FillData(cat);
                result.Add(cat);
                
                //category = category.ParentCategoryId != null ? _categoryGateway.SelectOne(category.ParentCategoryId.Value) : null;
            }
            result.Reverse();

            return result;
        }

        public void UpdateCategoriesSortOrder(ICollection<Category> categories)
        {
            using (var ts = new TransactionScope())
            {
                int i = 0;
                foreach (var cat in categories)
                {
                    RemoveObjectFromCache($"ChildCategoriesFor{null}");
                    var category = _categoryGateway.SelectOne(cat.HeartId);
                    category.SortOrder = i;
                    _categoryGateway.Update(category);
                    i++;
                    if (cat.ChildrenCategories.Any())
                    {
                        RemoveObjectFromCache($"ChildCategoriesFor{cat.HeartId}");
                        UpdateCategoriesSortOrder(cat.ChildrenCategories);
                    }
                }

                RemoveObjectFromCache("Categories");

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

        //public string GetCategoryCanonicalUrl(int categoryId)
        //{
        //    string cacheKey = GetCategoryIDCanonicalUrlCacheKey(categoryId);
        //    return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () =>
        //    {
        //        var cat = GetCategory(categoryId);
        //        return GetCategoryCanonicalUrl(cat.RelativeUrl);
        //    });
        //}
        //public string GetCategoryCanonicalUrl(string relativeUrl)
        //{
        //    string cacheKey = GetCategoryCanonicalUrlCacheKey(relativeUrl);
        //    return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () =>
        //    {
        //        string prefix = GetCannonicalCategoryUrlPrefix(relativeUrl);
        //        return String.IsNullOrEmpty(prefix) ? relativeUrl : String.Format("{0}/{1}", prefix, relativeUrl);
        //    });
        //}

        private void FillChildren(Category category, bool recursive = true)
        {
            var children = _categoryGateway.Select(category.HeartId);
            var resChildren = Mapper.Map<ICollection<Category>>(children);
            category.ChildrenCategories = resChildren;
            foreach (var child in resChildren)
            {
                
                child.ParentCategory = new IdNamePair<int>(category.HeartId, category.Name);
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
        }

        private string GetCategoryCanonicalUrlCacheKey(string url)
        {
            return String.Format(CATEGORY_CANNONICAL_URL_CACHE_KEY, url);
        }

        private string GetCategoryIDCanonicalUrlCacheKey(int categoryId)
        {
            return String.Format(CATEGORY_ID_CANNONICAL_URL_CACHE_KEY, categoryId);
        }

        //private string GetCannonicalCategoryUrlPrefix(string relativeUrl)
        //{
        //    string res = String.Empty;
        //    bool hasParent = false;
        //    {
        //        var rec = _categoryGateway.SelectOneByRelativeUrl(relativeUrl);
        //        if (rec.ParentCategoryId.HasValue)
        //        {
        //            var parent = _categoryGateway.SelectOne(rec.ParentCategoryId.Value);
        //            res = parent.RelativeUrl;
        //            hasParent = parent.ParentCategoryId.HasValue;
        //        }
        //    }
        //    if (String.IsNullOrEmpty(res) || !hasParent)
        //    {
        //        return res;
        //    }
        //    return String.Format("{0}/{1}", GetCannonicalCategoryUrlPrefix(res), res);
        //}

        //private void FillCanonicalUrls(ICollection<Category> categories)
        //{
        //    foreach (var category in categories)
        //    {
        //        category.CanonicalUrl = GetCategoryCanonicalUrl(category.RelativeUrl);
        //        if (category.ChildrenCategories.Any())
        //        {
        //            FillCanonicalUrls(category.ChildrenCategories);
        //        }
        //    }
        //}

        public void FillData(Category category)
        {
            var heart = _heartService.GetHeart(category.HeartId);
            category.FillHeart(heart);
            category.CanonicalUrl = _heartService.GetCanonicalUrl(category.HeartId);

            foreach(var child in category.ChildrenCategories)
            {
                FillData(child);
            }

        }

        public bool IsChild(int currentCategoryId, int parentCategoryId)
        {
            string cacheKey = $"ChildCategory:{currentCategoryId}:{parentCategoryId}";
            return GetFromCacheOrLoadAndAddToCache(cacheKey, () =>
            {
                var cat = _categoryGateway.SelectOne(currentCategoryId);
                if (cat.ParentCategoryId == parentCategoryId) return true;

                if (cat.ParentCategoryId.HasValue)
                {
                    return IsChild(cat.ParentCategoryId.Value, parentCategoryId);
                }

                return false;
            });
        }

        public Dictionary<int, int> GetCategoryStats()
        {
            string cacheKey = "CategoryStats";
            return GetFromCacheOrLoadAndAddToCache(cacheKey, () =>
            {
                var data = _goodsCategoryGateway.Select();
                Dictionary <int, int> dict = new Dictionary<int, int>();
                foreach (var item in data)
                {
                    if (dict.ContainsKey(item.CategoryId))
                    {
                        dict[item.CategoryId]++;
                    }
                    else
                    {
                        dict.Add(item.CategoryId, 1);
                    }
                }
                return dict;
            });
        }

        public IList<Category> GetChildCategories(int? parentCategoryId)
        {
            string cacheKey = $"ChildCategoriesFor{parentCategoryId}";
            var res = GetFromCacheOrLoadAndAddToCache(cacheKey, () =>
            {
                var dataCats = _categoryGateway.Select(parentCategoryId);
                Data.Models.Category parentCategory = null;
                if (parentCategoryId.HasValue)
                {
                    parentCategory = _categoryGateway.SelectOne(parentCategoryId.Value);
                }
                var cats = Mapper.Map<List<Category>>(dataCats);
                foreach (var category in cats)
                {
                    category.ParentCategory = parentCategory == null ? null : new IdNamePair<int>(parentCategory.HeartId, parentCategory.Name);
                    FillData(category);
                }
                SortCategories(cats);
                return cats;
            });

            
            return res;
        }
    }
}

