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
    public class FavouriteItemsService : BaseShopService, IFavouriteItemsService
    {
        private readonly FavouriteItemGateway _favouriteItemGateway = new FavouriteItemGateway();
        private readonly IShopService _shopService;

        public FavouriteItemsService(IShopService shopService)
        {
            _shopService = shopService;
        }

        public IList<FavouriteItem> GetFavouriteItems(Guid sessionId)
        {
            var dataItems = _favouriteItemGateway.Select(sessionId);
            var items = Mapper.Map<IList<FavouriteItem>>(dataItems);
            foreach (var item in items)
            {
                item.Item = _shopService.GetGoods(item.HeartId);
            }
            return items;
        }

        public bool IsItemInFavourites(Guid sessionId, int heartId)
        {
            return GetFavouriteItems(sessionId).Any(x => x.HeartId == heartId);
        }

        public void Add(Guid sessionId, int heartId)
        {
            _favouriteItemGateway.Insert(new Data.Models.FavouriteItem() { SessionId = sessionId, HeartId = heartId });
        }

        public void Delete(Guid sessionId, int heartId)
        {
            _favouriteItemGateway.Delete(new Data.Models.FavouriteItem() { SessionId = sessionId, HeartId = heartId });
        }
    }
}
