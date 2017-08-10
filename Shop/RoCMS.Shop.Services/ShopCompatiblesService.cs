using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Shop.Data.Models;
using CompatibleSet = RoCMS.Shop.Contract.Models.CompatibleSet;

namespace RoCMS.Shop.Services
{
    class ShopCompatiblesService: BaseShopService, IShopCompatiblesService
    {
        private readonly CompatibleSetGateway _compatibleSetGateway = new CompatibleSetGateway();
        private readonly CompatibleSetGoodsGateway _compatibleSetGoodsGateway = new CompatibleSetGoodsGateway();
        private readonly GoodsItemGateway _goodsItemGateway = new GoodsItemGateway();

        public CompatibleSet GetCompatibleSet(int compatibleSetId)
        {
            var dataRes = _compatibleSetGateway.SelectOne(compatibleSetId);
            var res = Mapper.Map<CompatibleSet>(dataRes);
            FillGoods(res);
            return res;
        }

        private void FillGoods(CompatibleSet set)
        {
            var goodsIds = _compatibleSetGoodsGateway.SelectByCompatibleSet(set.CompatibleSetId);
            var goodsIdNames =
                goodsIds
                .Select(x => new IdNamePair<int>(x.GoodsId, _goodsItemGateway.SelectOne(x.GoodsId).Name))
                .ToList();
            set.CompatibleGoods = goodsIdNames;
        }

        public int CreateCompatibleSet(CompatibleSet compatibleSet)
        {
            var dataRec = Mapper.Map<Data.Models.CompatibleSet>(compatibleSet);
            using (var ts = new TransactionScope())
            {
                int id = _compatibleSetGateway.Insert(dataRec);
                foreach (var compatibleGood in compatibleSet.CompatibleGoods)
                {

                    _compatibleSetGoodsGateway.Insert(new CompatibleSetGoods()
                    {
                        GoodsId = compatibleGood.ID,
                        CompatibleSetId = id
                    });
                }
                ts.Complete();
                return id;
            }
        }

        public void UpdateCompatibleSet(CompatibleSet compatibleSet)
        {
            var dataRec = Mapper.Map<Data.Models.CompatibleSet>(compatibleSet);
            _compatibleSetGateway.Update(dataRec);

            var oldGoods = _compatibleSetGoodsGateway.SelectByCompatibleSet(compatibleSet.CompatibleSetId);
            var newGoods = compatibleSet.CompatibleGoods;
            foreach (var oldGoodsItem in oldGoods)
            {
                if (newGoods.All(x => x.ID != oldGoodsItem.GoodsId))
                {
                    _compatibleSetGoodsGateway.Delete(oldGoodsItem);
                }
            }
            foreach (var newGoodsItem in newGoods)
            {
                var compatibleSetGoodsItem = new CompatibleSetGoods()
                {
                    GoodsId = newGoodsItem.ID,
                    CompatibleSetId = compatibleSet.CompatibleSetId
                };
                if (oldGoods.All(x => x.GoodsId != newGoodsItem.ID))
                {
                    _compatibleSetGoodsGateway.Insert(compatibleSetGoodsItem);
                }
            }

        }

        public void DeleteCompatibleSet(int compatibleSetId)
        {
            _compatibleSetGateway.Delete(compatibleSetId);
        }

        public IList<CompatibleSet> GetCompatibleSets()
        {
            var dataRes = _compatibleSetGateway.Select();
            var res = Mapper.Map<IList<CompatibleSet>>(dataRes);
            foreach (var compatibleSet in res)
            {
                FillGoods(compatibleSet);
            }
            return res;
        }
    }
}
