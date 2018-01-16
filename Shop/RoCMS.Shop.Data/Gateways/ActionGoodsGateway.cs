using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class ActionGoodsGateway: ShopBaseGateway
    {
        protected override string TableName => "Action_Goods";
        public void Insert(ActionGoods rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(ActionGoods rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public ICollection<ActionGoods> SelectByAction(int actionId)
        {
            return ExecSelect<ActionGoods>(GetProcedureString(), actionId);
        }
        public ICollection<ActionGoods> SelectByGoods(int heartId)
        {
            return ExecSelect<ActionGoods>(GetProcedureString(), heartId);
        }
    }
}
