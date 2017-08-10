using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Web.Contract.Models.Security;

namespace RoCMS.Shop.Services
{
    class CartDbService: BaseShopService, ICartService
    {
        private readonly CartItemGateway _cartItemGateway = new CartItemGateway();
        private readonly CartGateway _cartGateway = new CartGateway();
        private readonly IShopService _shopService;
        private readonly IShopClientService _shopClientService;

        public CartDbService(IShopService shopService, IShopClientService shopClientService)
        {
            _shopService = shopService;
            _shopClientService = shopClientService;
        }

        public Cart GetCart(Guid cartId)
        {
            var dataRes = _cartGateway.SelectOne(cartId);
            if (dataRes == null)
                return null;

            var res = Mapper.Map<Cart>(dataRes);

            var items = _cartItemGateway.SelectByCart(cartId);
            res.CartItems = Mapper.Map<List<CartItem>>(items);
            foreach (var cartItem in res.CartItems)
            {
                cartItem.GoodsItem = _shopService.GetGoods(cartItem.GoodsId);
            }
            return res;
        }

        public void AddItemToCart(Guid cartId, int goodsId, int count, int? packId)
        {
            var cart = _cartGateway.SelectOne(cartId);
            using (var ts = new TransactionScope())
            {
                if (cart == null)
                {
                    cart = new Data.Models.Cart() {CartId = cartId};
                    _cartGateway.Insert(cart);
                }
                else
                {
                    var item = _cartItemGateway.SelectByCart(cartId)
                        .SingleOrDefault(x => x.GoodsId == goodsId && x.CartId == cartId
                                              && (x.PackId == packId || x.PackId == null && packId == null));
                    if (item != null)
                    {
                        item.Quantity += count;
                        _cartItemGateway.Update(item);
                        ts.Complete();
                        return;
                    }
                }

                var newCartItem = new Data.Models.CartItem()
                {
                    CartId = cartId,
                    GoodsId = goodsId,
                    Quantity = count,
                    PackId = packId
                };
                _cartItemGateway.Insert(newCartItem);
                ts.Complete();
            }
        }

        public void RemoveItemFromCart(Guid cartId, int goodsId, int? packId)
        {
            var cartItem = _cartItemGateway.SelectByCart(cartId).FirstOrDefault(x => x.CartId == cartId && x.GoodsId == goodsId
                    && (x.PackId == packId || x.PackId == null && packId == null));
            if(cartItem != null)
            {
                _cartItemGateway.Delete(cartItem.CartItemId);
            }
        }

        public void UpdatItemCount(Guid cartId, int goodsId, int count, int? packId)
        {
            var cartItem = _cartItemGateway.SelectByCart(cartId).FirstOrDefault(x => x.CartId == cartId && x.GoodsId == goodsId
                    && (x.PackId == packId || x.PackId == null && packId == null));
            if(cartItem == null)
                return;
            cartItem.Quantity = count;
            _cartItemGateway.Update(cartItem);
        }

        public void RemoveCart(Guid cartId)
        {
            _cartGateway.Delete(cartId);
        }

        public CartSummary GetCartSummary(Guid cartId)
        {
            var cart = GetCart(cartId);
            if (cart == null)
                return new CartSummary();
            return new CartSummary() { Quantity = cart.Quantity, Summary = cart.Summary };
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
                discount = _shopClientService.GetRegularDiscountForClient(currentPrincipal.UserId);
            }
            var cart = _cartGateway.SelectOne(cartId);
            cart.TotalDiscount = discount;
            _cartGateway.Update(cart);

        }
    }
}
