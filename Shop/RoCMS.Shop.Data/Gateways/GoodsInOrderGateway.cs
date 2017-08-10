using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class GoodsInOrderGateway: ShopBaseGateway
    {
        public void Insert(GoodsInOrder rec)
        {
            Exec(GetProcedureString(), rec);
        }

        public void Update(GoodsInOrder rec)
        {
            Exec(GetProcedureString(), rec);
        }

        public ICollection<GoodsInOrder> SelectByOrder(int orderId)
        {
            return ExecSelect<GoodsInOrder>(GetProcedureString(), orderId);
        }
        public ICollection<GoodsInOrder> SelectByGoods(int goodsId)
        {
            return ExecSelect<GoodsInOrder>(GetProcedureString(), goodsId);
        }

        public void Delete(int id)
        {
            Exec(GetProcedureString(), id);
        }
    }
}
