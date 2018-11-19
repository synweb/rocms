using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class GoodsImageGateway: ShopBaseGateway
    {
        protected override string TableName => "Goods_Image";
        public void Insert(GoodsImage rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(GoodsImage rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Update(GoodsImage rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public ICollection<GoodsImage> SelectByGoods(int heartId)
        {
            return ExecSelect<GoodsImage>(GetProcedureString(), heartId);
        }
    }
}
