using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class GoodsSpecGateway: ShopBaseGateway
    {
        protected override string TableName => "Goods_Spec";

        public void Insert(GoodsSpec rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Update(GoodsSpec rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(GoodsSpec rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public ICollection<GoodsSpec> SelectByGoods(int goodsId)
        {
            return ExecSelect<GoodsSpec>(GetProcedureString(), goodsId);
        }
    }
}
