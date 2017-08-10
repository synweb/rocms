using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class GoodsPackGateway: ShopBaseGateway
    {
        protected override string TableName => "Goods_Pack";

        public void Insert(GoodsPack rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Update(GoodsPack rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(GoodsPack rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public ICollection<GoodsPack> SelectByGoods(int goodsId)
        {
            return ExecSelect<GoodsPack>(GetProcedureString(), goodsId);
        }
    }
}
