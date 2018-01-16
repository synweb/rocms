using System;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface ICartService
    {
        Cart GetCart(Guid cartId);
        void AddItemToCart(Guid cartId, int heartId, int count, int? packId);
        void RemoveItemFromCart(Guid cartId, int heartId, int? packId);
        void UpdatItemCount(Guid cartId, int heartId, int count, int? packId);

        void RemoveCart(Guid cartId);

        CartSummary GetCartSummary(Guid cartId);
        void UpdateUserDiscount(Guid cartId);
    }
}
