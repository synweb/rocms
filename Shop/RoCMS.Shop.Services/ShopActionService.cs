using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Shop.Data.Models;
using RoCMS.Web.Contract.Services;
using Action = RoCMS.Shop.Contract.Models.Action;

namespace RoCMS.Shop.Services
{
    class ShopActionService: BaseShopService, IShopActionService
    {
        private readonly ActionGateway _actionGateway = new ActionGateway();
        private readonly ActionGoodsGateway _actionGoodsGateway = new ActionGoodsGateway();
        private readonly ActionCategoryGateway _actionCategoryGateway = new ActionCategoryGateway();
        private readonly CategoryGateway _categoryGateway = new CategoryGateway();
        private readonly ActionManufacturerGateway _actionManufacturerGateway = new ActionManufacturerGateway();
        private readonly GoodsItemGateway _goodsGateway = new GoodsItemGateway();
        private readonly GoodsCategoryGateway _goodsCategoryGateway = new GoodsCategoryGateway();
        private readonly IHeartService _heartService;


        private readonly IShopManufacturerService _shopManufacturerService;

        public ShopActionService(IShopManufacturerService shopManufacturerService, IHeartService heartService)
        {
            _shopManufacturerService = shopManufacturerService;
            _heartService = heartService;
        }

        public Action GetAction(int actionId)
        {
            var dataRes = _actionGateway.SelectOne(actionId);
            var res = Mapper.Map<Action>(dataRes);
            FillData(res);
            return res;
        }

        private void FillData(Action action)
        {
            var heart = _heartService.GetHeart(action.HeartId);
            action.FillHeart(heart);

            FillGoods(action);
            FillCats(action);
            FillManufaturers(action);
        }

        private void FillManufaturers(Action action)
        {
            var manIds = _actionManufacturerGateway.SelectByAction(action.HeartId).Select(x => x.ManufacturerId);
            var mans = manIds.Select(x => _shopManufacturerService.GetManufacturer(x));
            var idNames = mans.Select(x => new IdNamePair<int>(x.HeartId, x.Name)).ToList();
            action.Manufacturers = idNames;
        }

        private void FillCats(Action action)
        {
            var catIds = _actionCategoryGateway.SelectByAction(action.HeartId).Select(x => x.CategoryId);
            var cats = catIds.Select(x => _categoryGateway.SelectOne(x));
            var idNames = cats.Select(x => new IdNamePair<int>(x.HeartId, x.Name)).ToList();
            action.Categories = idNames;
        }

        private void FillGoods(Action action)
        {
            var ids = _actionGoodsGateway.SelectByAction(action.HeartId).Select(x => x.HeartId);
            var goods = ids.Select(x => _goodsGateway.SelectOne(x));
            var idNames = goods.Select(x => new IdNamePair<int>(x.HeartId, x.Name)).ToList();
            action.Goods = idNames;
        }

        public int CreateAction(Action action)
        {
            action.Type = action.GetType().FullName;

            var dataRec = Mapper.Map<Data.Models.Action>(action);
            using (var ts = new TransactionScope())
            {

                int id = action.HeartId = dataRec.HeartId = _heartService.CreateHeart(action);

                _actionGateway.Insert(dataRec);


                foreach (var catId in action.Categories.Select(x => x.ID))
                {
                    _actionCategoryGateway.Insert(new ActionCategory() {ActionId = id, CategoryId = catId});
                }
                foreach (var goodsId in action.Goods.Select(x => x.ID))
                {
                    _actionGoodsGateway.Insert(new ActionGoods() { ActionId = id, HeartId = goodsId });
                }
                foreach (var manId in action.Manufacturers.Select(x => x.ID))
                {
                    _actionManufacturerGateway.Insert(new ActionManufacturer() { ActionId = id, ManufacturerId = manId });
                }
                ts.Complete();
                return id;
            }
        }

        public void UpdateAction(Action action)
        {
            var dataAction = Mapper.Map<Data.Models.Action>(action);
            using (var ts = new TransactionScope())
            {
                int actionId = action.HeartId;

                _heartService.UpdateHeart(action);

                _actionGateway.Update(dataAction);

                var oldCats = _actionCategoryGateway.SelectByAction(actionId);
                var newCats = action.Categories;
                foreach (var oldCat in oldCats)
                {
                    if (newCats.All(x => x.ID != oldCat.CategoryId))
                    {
                        _actionCategoryGateway.Delete(oldCat);
                    }
                }
                foreach (var newCat in newCats)
                {
                    if (oldCats.All(x => x.CategoryId != newCat.ID))
                    {
                        _actionCategoryGateway.Insert(new ActionCategory()
                        {
                            ActionId = actionId,
                            CategoryId = newCat.ID
                        });
                    }
                }

                var oldGoods = _actionGoodsGateway.SelectByAction(actionId);
                var newGoods = action.Goods;
                foreach (var old in oldGoods)
                {
                    if (newGoods.All(x => x.ID != old.HeartId))
                    {
                        _actionGoodsGateway.Delete(old);
                    }
                }
                foreach (var @new in newGoods)
                {
                    if (oldGoods.All(x => x.HeartId != @new.ID))
                    {
                        _actionGoodsGateway.Insert(new ActionGoods()
                        {
                            ActionId = actionId,
                            HeartId = @new.ID
                        });
                    }
                }

                var oldMans = _actionManufacturerGateway.SelectByAction(actionId);
                var newMans = action.Manufacturers;

                foreach (var oldMan in oldMans)
                {
                    if (newMans.All(x => x.ID != oldMan.ManufacturerId))
                    {
                        _actionManufacturerGateway.Delete(oldMan);
                    }
                }

                foreach (var newMan in newMans)
                {
                    if (oldMans.All(x => x.ManufacturerId != newMan.ID))
                    {
                        _actionManufacturerGateway.Insert(new ActionManufacturer()
                        {
                            ActionId = actionId,
                            ManufacturerId = newMan.ID
                        });
                    }
                }
                ts.Complete();
            }
        }

        public void DeleteAction(int actionId)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                _actionGateway.Delete(actionId);
                _heartService.DeleteHeart(actionId);

                ts.Complete();
            }
        }

        public IList<Action> GetActions()
        {
            var dataRes = _actionGateway.Select();
            var res = Mapper.Map<IList<Action>>(dataRes);
            foreach (var action in res)
            {
                FillData(action);
            }
            return res;
        }

        public bool ActionExists(int id)
        {
            return _actionGateway.Exists(id);
        }

        public IList<ActionShortInfo> GetActiveActionsForGoodsItem(int goodsItemId)
        {
            var goodsItem = _goodsGateway.SelectOne(goodsItemId);
            var actionIds = _actionGoodsGateway.SelectByGoods(goodsItemId).Select(x => x.ActionId);

            if (goodsItem.ManufacturerId.HasValue)
            {
                var manufacturerActionIds = _actionManufacturerGateway.SelectByManufacturer(goodsItem.ManufacturerId.Value)
                    .Select(x => x.ActionId);
                actionIds = actionIds.Concat(manufacturerActionIds);
            }


            var catIds = _goodsCategoryGateway.SelectByGoods(goodsItemId).Select(x => x.CategoryId).ToList();
            foreach (var catId in catIds.ToArray())
            {
                //TODO: можно перенести в CatService
                FillCategoryIdsWithParents(catId, catIds);
            }
            if (catIds.Any())
            {
                var catActionIds = catIds.SelectMany(x => _actionCategoryGateway.SelectByCategory(x))
                    .Select(x => x.ActionId);
                actionIds = actionIds.Concat(catActionIds);
            }

            var actions = actionIds.Distinct().Select(x => _actionGateway.SelectOne(x));
            var res = actions.Where(x => x.Active &&
                                           (!x.DateOfEnding.HasValue || x.DateOfEnding >= DateTime.UtcNow))
                                           .Select(x => new ActionShortInfo(x.HeartId, x.Name, x.Discount))
                                           .ToList();
            return res;
        }

        /// <summary>
        /// Рекурсивное заполнение списка айдишниками предков
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="overallIds"></param>
        private void FillCategoryIdsWithParents(int categoryId, List<int> overallIds)
        {
            var cat = _categoryGateway.SelectOne(categoryId);
            if (cat.ParentCategoryId.HasValue)
            {
                overallIds.Add(cat.ParentCategoryId.Value);
                FillCategoryIdsWithParents(cat.ParentCategoryId.Value, overallIds);
            }
        }

        public IList<Action> GetActionsForSlider()
        {
            // можно оптимизировать отдельной хранимкой
            // можно закэшировать
            return GetActions().Where(x => x.Active &&
                                           (!x.DateOfEnding.HasValue || x.DateOfEnding >= DateTime.UtcNow) &&
                                           x.ShowInSlider).ToList();
        }

        public IList<Action> GetActiveActions()
        {
            // можно оптимизировать отдельной хранимкой
            // можно закэшировать
            return GetActions().Where(x => x.Active &&
                                           (!x.DateOfEnding.HasValue || x.DateOfEnding >= DateTime.UtcNow)
                                           ).ToList();
        }
    }
}
