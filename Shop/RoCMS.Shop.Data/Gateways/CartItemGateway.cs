using System;
using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class CartItemGateway: ShopBaseGateway
    {
        public ICollection<CartItem> SelectByCart(Guid cartId)
        {
            return ExecSelect<CartItem>(GetProcedureString(), cartId);
        }

        public void Update(CartItem item)
        {
            Exec(GetProcedureString(), item);
        }

        public void Insert(CartItem item)
        {
            Exec(GetProcedureString(), item);
        }

        public void Delete(int cartItemId)
        {
            Exec(GetProcedureString(), cartItemId);
        }
    }
}
