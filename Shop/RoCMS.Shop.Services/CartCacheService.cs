using System;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using RoCMS.Base.Helpers;
using RoCMS.Base.Services;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Models.Security;

namespace RoCMS.Shop.Services
{
    public class CartCacheService: BaseCacheService, ICartService
    {
        private IShopService _shopService;
        private IShopClientService _clientService;

        public CartCacheService(IShopService shopService, IShopClientService clientService)
        {
            _shopService = shopService;
            _clientService = clientService;
            InitCache("CartServiceMemoryCache");
        }

        public Cart GetCart(Guid cartId)
        {
            var result = CacheContainsObject(cartId.ToString()) ? GetFromCache<Cart>(cartId.ToString()) : new Cart() { CartId = cartId };
            RefreshCacheExpirationForObject(cartId.ToString());
            return result;
        }


        public void AddItemToCart(Guid cartId, int goodsId, int count, int? packId)
        {
            var cart = GetCart(cartId);
            if (cart.CartItems.Any(x => x.GoodsItem.GoodsId == goodsId && x.PackId == packId))
            {
                var goods = cart.CartItems.First(x => x.GoodsItem.GoodsId == goodsId);
                goods.Quantity += count;
            }
            else
            {
                cart.CartItems.Add(new CartItem()
                {
                    GoodsItem = _shopService.GetGoods(goodsId),
                    Quantity = count,
                    PackId = packId
                });
            }
            AddOrUpdateCacheObject(cartId.ToString(), cart);
        }

        public void RemoveItemFromCart(Guid cartId, int goodsId, int? packId)
        {
            var cart = GetFromCache<Cart>(cartId.ToString()) ?? new Cart() { CartId = cartId };
            cart.CartItems.RemoveAll(x => x.GoodsItem.GoodsId == goodsId && x.PackId == packId);
            AddOrUpdateCacheObject(cartId.ToString(), cart);
        }

        public void UpdatItemCount(Guid cartId, int goodsId, int count, int? packId)
        {
            var cart = GetFromCache<Cart>(cartId.ToString()) ?? new Cart() { CartId = cartId };
            cart.CartItems.First(x => x.GoodsItem.GoodsId == goodsId && x.PackId == packId).Quantity = count;
            AddOrUpdateCacheObject(cartId.ToString(), cart);
        }

        public void RemoveCart(Guid cartId)
        {
            RemoveObjectFromCache(cartId.ToString());
        }

        public CartSummary GetCartSummary(Guid cartId)
        {
            var cart = GetCart(cartId);
            return new CartSummary() {Quantity = cart.Quantity, Summary = cart.Summary};
        }

        public void UpdateUserDiscount(Guid cartId)
        {
            RoPrincipal currentPrincipal = Thread.CurrentPrincipal as RoPrincipal;
            decimal discount;
            if (currentPrincipal == null)
            {
                discount = 0;
            }
            else
            {
                discount = _clientService.GetRegularDiscountForClient(currentPrincipal.UserId);
            }
            var cart = GetCart(cartId);

            cart.TotalDiscount = discount;

            AddOrUpdateCacheObject(cartId.ToString(), cart);
        }

        protected override int CacheExpirationInMinutes
        {
            get { return AppSettingsHelper.HoursToExpireCartCache * 60; }
        }
    }
}
