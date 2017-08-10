using System;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface ICartService
    {
        Cart GetCart(Guid cartId);
        void AddItemToCart(Guid cartId, int goodsId, int count, int? packId);
        void RemoveItemFromCart(Guid cartId, int goodsId, int? packId);
        void UpdatItemCount(Guid cartId, int goodsId, int count, int? packId);

        void RemoveCart(Guid cartId);

        CartSummary GetCartSummary(Guid cartId);
        void UpdateUserDiscount(Guid cartId);
    }
}
