using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class GoodsCategoryGateway: ShopBaseGateway
    {
        protected override string TableName => "Goods_Category";
        public void Insert(GoodsCategory rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(GoodsCategory rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public ICollection<GoodsCategory> SelectByGoods(int heartId)
        {
            return ExecSelect<GoodsCategory>(GetProcedureString(), heartId);
        }
        public ICollection<GoodsCategory> SelectByCategory(int categoryId)
        {
            return ExecSelect<GoodsCategory>(GetProcedureString(), categoryId);
        }
    }
}
