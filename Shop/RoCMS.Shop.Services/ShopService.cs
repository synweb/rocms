using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.Data.Gateways;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Models.Exceptions;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Shop.Data.Models;
using RoCMS.Web.Contract.Services;
using FilterCollections = RoCMS.Shop.Contract.Models.FilterCollections;
using GoodsFilter = RoCMS.Shop.Contract.Models.GoodsFilter;
using GoodsItem = RoCMS.Shop.Contract.Models.GoodsItem;
using GoodsPack = RoCMS.Shop.Contract.Models.GoodsPack;
using Spec = RoCMS.Shop.Contract.Models.Spec;

namespace RoCMS.Shop.Services
{
    class ShopService : BaseShopService, IShopService
    {

        private const string GOODS_CANNONICAL_URL_CACHE_KEY = "GoodsCannonical:{0}";

        private readonly ILogService _logService;
        private readonly IShopActionService _shopActionService;
        private readonly IShopCategoryService _shopCategoryService;
        private readonly IShopSpecService _shopSpecService;
        private readonly IShopCompatiblesService _shopCompatiblesService;
        private readonly IShopManufacturerService _shopManufacturerService;
        private readonly IShopPackService _shopPackService;

        private readonly GoodsItemGateway _goodsItemGateway = new GoodsItemGateway();
        private readonly GoodsSpecGateway _goodsSpecGateway = new GoodsSpecGateway();
        private readonly GoodsPackGateway _goodsPackGateway = new GoodsPackGateway();
        private readonly GoodsImageGateway _goodsImageGateway = new GoodsImageGateway();
        private readonly CompatibleSetGoodsGateway _compatibleSetGoodsGateway = new CompatibleSetGoodsGateway();
        private readonly GoodsCategoryGateway _goodsCategoryGateway = new GoodsCategoryGateway();
        private readonly CategoryGateway _categoryGateway = new CategoryGateway();
        private readonly GoodsReviewGateway _goodsReviewGateway = new GoodsReviewGateway();
        private readonly ActionGoodsGateway _actionGoodsGateway = new ActionGoodsGateway();
        private readonly CountryGateway _countryGateway = new CountryGateway();

        public ShopService(ILogService logService, IShopActionService shopActionService, IShopCategoryService shopCategoryService, IShopSpecService shopSpecService, IShopCompatiblesService shopCompatiblesService, IShopPackService shopPackService, IShopManufacturerService shopManufacturerService)
        {
            _logService = logService;
            _shopActionService = shopActionService;
            _shopCategoryService = shopCategoryService;
            _shopSpecService = shopSpecService;
            _shopCompatiblesService = shopCompatiblesService;
            _shopPackService = shopPackService;
            _shopManufacturerService = shopManufacturerService;
            InitCache("ShopService");

            //GenerateRelativeUrls();
        }

        //Для генерации урлов в уже существующих товарах и категориях
        //private void GenerateRelativeUrls()
        //{
        //    using (var context = Context)
        //    {
        //        foreach (var item in context.CategorySet)
        //        {
        //            if (String.IsNullOrEmpty(item.RelativeUrl))
        //            {
        //                item.RelativeUrl = String.Format("{0}-{1}", TranslitHelper.ToTranslitedUrl(item.Name, 250),
        //                    item.CategoryId);
        //            }
        //            else
        //            {
        //                item.RelativeUrl = Regex.Replace(item.RelativeUrl, @"--+", "-");
        //            }
        //        }


        //        foreach (var item in context.GoodsSet)
        //        {
        //            if (String.IsNullOrEmpty(item.RelativeUrl))
        //            {
        //                item.RelativeUrl = String.Format("{0}-{1}", TranslitHelper.ToTranslitedUrl(item.Name, 250), item.GoodsId);
        //            }
        //            else
        //            {
        //                item.RelativeUrl = Regex.Replace(item.RelativeUrl, @"--+", "-");
        //            }
        //        }

        //        context.SaveChanges();
        //    }
        //}

        public GoodsItem GetGoods(int goodsId, bool activeActionsOnly = true)
        {
            var goods = _goodsItemGateway.SelectOne(goodsId);
            if (goods == null || goods.Deleted)
            {
                throw new GoodsNotFoundException(goodsId);
            }
            var res = Mapper.Map<GoodsItem>(goods);
            FillData(res, activeActionsOnly);
            return res;
        }

        private void FillData(GoodsItem goodsItem, bool activeActionsOnly = true)
        {
            FillImages(goodsItem);
            FillCompatibles(goodsItem);
            FillCats(goodsItem);
            FillActions(goodsItem);
            FillPacks(goodsItem);
            FillSpecs(goodsItem);
            FillManufacturers(goodsItem);
            if (activeActionsOnly)
            {
                goodsItem.Actions.Clear();
                goodsItem.Actions = _shopActionService.GetActiveActionsForGoodsItem(goodsItem.GoodsId);
                goodsItem.CannonicalUrl = goodsItem.Categories.Any()
                    ? GetGoodsCannonicalUrl(goodsItem.RelativeUrl, goodsItem.Categories.First().ID)
                    : goodsItem.RelativeUrl;
            }
        }

        private void FillManufacturers(GoodsItem goodsItem)
        {
            if (goodsItem.ManufacturerId.HasValue)
            {
                goodsItem.Manufacturer = _shopManufacturerService.GetManufacturer(goodsItem.ManufacturerId.Value);
            }
            if (goodsItem.SupplierId.HasValue)
            {
                goodsItem.Supplier = _shopManufacturerService.GetManufacturer(goodsItem.SupplierId.Value);
            }
        }

        private void FillSpecs(GoodsItem goodsItem)
        {
            var dataVals = _goodsSpecGateway.SelectByGoods(goodsItem.GoodsId);
            var vals = Mapper.Map<IList<SpecValue>>(dataVals);
            foreach (var specValue in vals)
            {
                specValue.Spec = _shopSpecService.GetSpec(specValue.SpecId);
            }
            goodsItem.GoodsSpecs = vals;
        }

        private void FillPacks(GoodsItem goodsItem)
        {
            var dataVals = _goodsPackGateway.SelectByGoods(goodsItem.GoodsId);
            var vals = Mapper.Map<IList<GoodsPack>>(dataVals);
            foreach (var goodsPack in vals)
            {
                goodsPack.PackInfo = _shopPackService.GetPack(goodsPack.PackId);
            }
            goodsItem.Packs = vals;
        }

        private void FillActions(GoodsItem goodsItem)
        {
            goodsItem.Actions = _shopActionService.GetActiveActionsForGoodsItem(goodsItem.GoodsId);
        }

        private void FillCats(GoodsItem goodsItem)
        {
            var catsIds = _goodsCategoryGateway.SelectByGoods(goodsItem.GoodsId).Select(x => x.CategoryId);
            var cats = catsIds.Select(x => _categoryGateway.SelectOne(x));
            var idNames = Mapper.Map<IList<IdNamePair<int>>>(cats);
            goodsItem.Categories = idNames;
        }

        private void FillCompatibles(GoodsItem goodsItem)
        {
            var compIds = _compatibleSetGoodsGateway.SelectByGoods(goodsItem.GoodsId).Select(x => x.CompatibleSetId);
            var comps = compIds.Select(x => _shopCompatiblesService.GetCompatibleSet(x)).ToList();
            goodsItem.CompatibleGoods = comps;
        }

        private void FillImages(GoodsItem goodsItem)
        {
            var imageIds = _goodsImageGateway.SelectByGoods(goodsItem.GoodsId).Select(x => x.ImageId);
            goodsItem.Images = imageIds.ToList();
        }

        public GoodsItem GetGoods(string relativeUrl, bool activeActionsOnly = true)
        {
            var goods = _goodsItemGateway.SelectOneByRelativeUrl(relativeUrl);
            if (goods == null)
            {
                throw new GoodsNotFoundException(relativeUrl);
            }
            var res = Mapper.Map<GoodsItem>(goods);
            FillData(res, activeActionsOnly);
            return res;
        }

        public int CreateGoods(GoodsItem goods)
        {
            using (var ts = new TransactionScope())
            {
                var dataGoods = Mapper.Map<Data.Models.GoodsItem>(goods);
                dataGoods.SearchDescription = SearchHelper.ToSearchIndexText(dataGoods.HtmlDescription);
                int id = _goodsItemGateway.Insert(dataGoods);
                foreach (var goodsCategory in goods.Categories)
                {
                    _goodsCategoryGateway.Insert(new GoodsCategory()
                    {CategoryId = goodsCategory.ID, GoodsId = id});
                }
                foreach (var goodsImageId in goods.Images)
                {
                    _goodsImageGateway.Insert(new GoodsImage()
                    { ImageId = goodsImageId, GoodsId = id });
                }
                foreach (var goodsPack in goods.Packs)
                {
                    var dataGoodsPack = Mapper.Map<Data.Models.GoodsPack>(goodsPack);
                    dataGoodsPack.GoodsId = id;
                    _goodsPackGateway.Insert(dataGoodsPack);
                }
                foreach (var compatibleGoods in goods.CompatibleGoods)
                {
                    _compatibleSetGoodsGateway.Insert(new CompatibleSetGoods()
                    {CompatibleSetId = compatibleGoods.CompatibleSetId, GoodsId = id});
                }
                foreach (var specVal in goods.GoodsSpecs)
                {
                    var dataGoodsSpec = Mapper.Map<GoodsSpec>(specVal);
                    _goodsSpecGateway.Insert(dataGoodsSpec);
                }
                ts.Complete();
                return id;
            }
        }

        public void UpdateGoods(GoodsItem goods)
        {
            int goodsId = goods.GoodsId;
            var dataGoods = Mapper.Map<Data.Models.GoodsItem>(goods);
            dataGoods.SearchDescription = SearchHelper.ToSearchIndexText(dataGoods.HtmlDescription);
            using (var ts = new TransactionScope())
            {
                _goodsItemGateway.Update(dataGoods);

                var oldCats = _goodsCategoryGateway.SelectByGoods(goodsId);
                var newCats = goods.Categories;
                foreach (var oldCat in oldCats)
                {
                    if (newCats.All(x => x.ID != oldCat.CategoryId))
                    {
                        _goodsCategoryGateway.Delete(oldCat);
                    }
                }
                foreach (var newCat in newCats)
                {
                    if (oldCats.All(x => x.CategoryId != newCat.ID))
                    {
                        _goodsCategoryGateway.Insert(new GoodsCategory()
                        {
                            GoodsId = goodsId,
                            CategoryId = newCat.ID
                        });
                    }
                }

                var oldSpecs = _goodsSpecGateway.SelectByGoods(goodsId);
                var newSpecs = goods.GoodsSpecs;
                foreach (var oldSpec in oldSpecs)
                {
                    if (newSpecs.All(x => x.SpecId != oldSpec.SpecId))
                    {
                        _goodsSpecGateway.Delete(oldSpec);
                    }
                }
                foreach (var newSpec in newSpecs)
                {
                    var dataSpecVal = Mapper.Map<GoodsSpec>(newSpec);
                    if (oldSpecs.All(x => x.SpecId != newSpec.SpecId))
                    {
                        _goodsSpecGateway.Insert(dataSpecVal);
                    }
                    else
                    {
                        _goodsSpecGateway.Update(dataSpecVal);
                    }
                }

                var oldImages = _goodsImageGateway.SelectByGoods(goodsId);
                var newImages = goods.Images;
                foreach (var oldImage in oldImages)
                {
                    if (newImages.All(x => !Equals(x, oldImage.ImageId)))
                    {
                        _goodsImageGateway.Delete(oldImage);
                    }
                }
                foreach (var newImage in newImages)
                {
                    if (oldImages.All(x => !Equals(x.ImageId, newImage)))
                    {
                        _goodsImageGateway.Insert(new GoodsImage()
                        {
                            GoodsId = goodsId,
                            ImageId = newImage
                        });
                    }
                }

                var oldPacks = _goodsPackGateway.SelectByGoods(goodsId);
                var newPacks = goods.Packs;
                foreach (var oldPack in oldPacks)
                {
                    if (newPacks.All(x => x.PackId != oldPack.PackId))
                    {
                        _goodsPackGateway.Delete(oldPack);
                    }
                }
                foreach (var newPack in newPacks)
                {
                    var dataPackVal = Mapper.Map<Data.Models.GoodsPack>(newPack);
                    dataPackVal.GoodsId = goodsId;
                    if (oldPacks.All(x => x.PackId != newPack.PackId))
                    {
                        _goodsPackGateway.Insert(dataPackVal);
                    }
                    else
                    {
                        _goodsPackGateway.Update(dataPackVal);
                    }
                }

                var oldActions = _actionGoodsGateway.SelectByGoods(goodsId);
                var newActions = goods.Actions;
                foreach (var oldAction in oldActions)
                {
                    if (newActions.All(x => x.ID != oldAction.ActionId))
                    {
                        _actionGoodsGateway.Delete(oldAction);
                    }
                }
                foreach (var newAction in newActions)
                {
                    if (oldActions.All(x => x.ActionId != newAction.ID))
                    {
                        _actionGoodsGateway.Insert(new ActionGoods() {ActionId = newAction.ID, GoodsId = goodsId});
                    }
                }

                var oldComps = _compatibleSetGoodsGateway.SelectByGoods(goodsId);
                var newComps = goods.CompatibleGoods;
                foreach (var oldComp in oldComps)
                {
                    if (newComps.All(x => x.CompatibleSetId != oldComp.CompatibleSetId))
                    {
                        _compatibleSetGoodsGateway.Delete(oldComp);
                    }
                }
                foreach (var newComp in newComps)
                {
                    if (oldComps.All(x => x.CompatibleSetId != newComp.CompatibleSetId))
                    {
                        _compatibleSetGoodsGateway.Insert(new CompatibleSetGoods()
                        {
                            GoodsId = goodsId,
                            CompatibleSetId = newComp.CompatibleSetId
                        });
                    }
                }
                ts.Complete();
            }
        }

        public void DeleteGoods(int goodsId)
        {
            using (var ts = new TransactionScope())
            {
                _goodsItemGateway.Delete(goodsId);
                foreach (var compatibleSetGoods in _compatibleSetGoodsGateway.SelectByGoods(goodsId))
                {
                    _compatibleSetGoodsGateway.Delete(compatibleSetGoods);
                }
                foreach (var goodsCategory in _goodsCategoryGateway.SelectByGoods(goodsId))
                {
                    _goodsCategoryGateway.Delete(goodsCategory);
                }
                foreach (var goodsImage in _goodsImageGateway.SelectByGoods(goodsId))
                {
                    _goodsImageGateway.Delete(goodsImage);
                }
                foreach (var goodsReview in _goodsReviewGateway.SelectByGoods(goodsId))
                {
                    _goodsReviewGateway.Delete(goodsReview.GoodsReviewId);
                }
                foreach (var actionGoods in _actionGoodsGateway.SelectByGoods(goodsId))
                {
                    _actionGoodsGateway.Delete(actionGoods);
                }
                // упаковки не удаляем, так как они, возможно, понадобятся для истории заказов
                foreach (var goodsSpec in _goodsSpecGateway.SelectByGoods(goodsId))
                {
                    _goodsSpecGateway.Delete(goodsSpec);
                }
                ts.Complete();
            }
        }

        public IList<GoodsItem> GetGoodsSet(GoodsFilter filter, int startIndex, int count, out int totalCount, out FilterCollections collections,
            bool activeActionsOnly = true)
        {
            var dataFilter = Mapper.Map<Data.Models.GoodsFilter>(filter);
            var goodsIds = _goodsItemGateway.FilterGoods(dataFilter, startIndex, count, out totalCount);
            collections = new FilterCollections();
            collections.Manufacturers = _goodsItemGateway.GetManufacturers(goodsIds);
            collections.Countries = _goodsItemGateway.GetCountries(goodsIds);
            collections.Packs = _goodsItemGateway.GetPacks(goodsIds);
            var specValues = _goodsItemGateway.GetSpecs(goodsIds);

            foreach (var spec in specValues)
            {
                var colSpec = collections.SpecValues.Keys.SingleOrDefault(x => x.SpecId == spec.Key);
                if (colSpec == null)
                {
                    var filterSpec = _shopSpecService.GetSpec(spec.Key);
                    if (filterSpec == null)
                    {
                        continue;
                    }
                    collections.SpecValues.Add(filterSpec, new List<string>() { spec.Value });
                }
                else
                {
                    collections.SpecValues[colSpec].Add(spec.Value);
                }
            }


            var goodsPage = startIndex > 1 ? goodsIds.Skip(startIndex - 1).Take(count) : goodsIds.Take(count);

            var result = new List<GoodsItem>();
            foreach (int goodsId in goodsPage)
            {
                result.Add(GetGoods(goodsId, activeActionsOnly));
            }
            return result;
            //TODO: кэш
        }

        private string GetGoodsFilterCacheKey(GoodsFilter filter, bool activeActionsOnly)
        {
            string cacheKey =
                $"GOODS:{String.Join(",", filter.Articles)}:{String.Join(",", filter.CategoryIds)}:{String.Join(",", filter.Countries)}:{String.Join(",", filter.ManufacturerIds)}:{String.Join(",", filter.Packs)}:{filter.SearchPattern}:{filter.SortBy}:{filter.ClientMode}";
            return cacheKey;
        }

        private string GetGoodsPageFilterCacheKey(GoodsFilter filter, int startIndex, int count, bool activeActionsOnly)
        {
            string cacheKey = $"GOODSPAGE:{GetGoodsFilterCacheKey(filter, activeActionsOnly)}:{startIndex}:{count}";
            return cacheKey;
        }

        public int[] GetGoodsIds(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IList<Country> GetCountries()
        {
            // TODO: перенести в ядро
            return Mapper.Map<IList<Country>>(_countryGateway.Select());
        }

        public IList<GoodsItem> GetBestSellers(int count)
        {
            throw new NotImplementedException();
        }

        public IList<GoodsItem> GetNewGoodsItems(int count)
        {
            String cacheKey = String.Format("NEWITEMS:{0}", count);
            return GetFromCacheOrLoadAndAddToCache(cacheKey, () =>
            {
                {
                    var goods = _goodsItemGateway.SelectNew(count);
                    var result = Mapper.Map<IList<GoodsItem>>(goods);
                    foreach (var goodsItem in result)
                    {
                        FillData(goodsItem);
                    }
                    return result;
                }
            });
        }

        public bool GoodsExists(int id)
        {
            return _goodsItemGateway.Exists(id);
        }

        public bool GoodsExists(string relativeUrl)
        {
            return _goodsItemGateway.ExistsByRelativeUrl(relativeUrl);
        }

        public IList<GoodsItem> GetRecommendedGoods(int count, int[] categoryids, int currentGoodsId)
        {
            throw new NotImplementedException();
        }

        private string GetGoodsCannonicalUrlCacheKey(string url)
        {
            return String.Format(GOODS_CANNONICAL_URL_CACHE_KEY, url);
        }

        public string GetGoodsCannonicalUrl(string relativeUrl, int categoryId)
        {
            string cacheKey = GetGoodsCannonicalUrlCacheKey(relativeUrl);
            return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () =>
            {

                string prefix = _shopCategoryService.GetCategoryCannonicalUrl(categoryId);
                return String.IsNullOrEmpty(prefix) ? relativeUrl : String.Format("{0}/{1}", prefix, relativeUrl);
            });

        }
    }
}
