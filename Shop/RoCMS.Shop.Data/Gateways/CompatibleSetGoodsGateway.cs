using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class CompatibleSetGoodsGateway:ShopBaseGateway
    {
        protected override string TableName => "CompatibleSet_Goods";

        public void Insert(CompatibleSetGoods rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(CompatibleSetGoods rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public ICollection<CompatibleSetGoods> SelectByGoods(int goodsId)
        {
            return ExecSelect<CompatibleSetGoods>(GetProcedureString(), goodsId);
        }
        public ICollection<CompatibleSetGoods> SelectByCompatibleSet(int setId)
        {
            return ExecSelect<CompatibleSetGoods>(GetProcedureString(), setId);
        }
    }
}
