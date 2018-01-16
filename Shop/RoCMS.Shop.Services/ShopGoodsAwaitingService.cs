using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;

namespace RoCMS.Shop.Services
{
    class ShopGoodsAwaitingService: BaseShopService, IShopGoodsAwaitingService
    {
        private readonly GoodsAwaitingGateway _goodsAwaitingGateway = new GoodsAwaitingGateway();
        public void SendNotifications(int heartId)
        {
            throw new NotImplementedException();
        }

        public void SendNotifications(int[] heartId)
        {
            throw new NotImplementedException();
        }

        public int CreateGoodsAwaiting(GoodsAwaiting goodsAwaiting)
        {
            var dataRec = Mapper.Map<Data.Models.GoodsAwaiting>(goodsAwaiting);
            int id = _goodsAwaitingGateway.Insert(dataRec);
            return id;
        }

        public void DeleteGoodsAwaiting(int goodsAwaitingId)
        {
            _goodsAwaitingGateway.Delete(goodsAwaitingId);
        }
    }
}
