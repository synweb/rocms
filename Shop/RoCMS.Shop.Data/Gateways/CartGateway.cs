using System;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class CartGateway:ShopBaseGateway
    {
        public void Insert(Cart rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Update(Cart rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(Guid id)
        {
            Exec(GetProcedureString(), id);
        }
        public Cart SelectOne(Guid id)
        {
            return Exec<Cart>(GetProcedureString(), id);
        }
    }
}
