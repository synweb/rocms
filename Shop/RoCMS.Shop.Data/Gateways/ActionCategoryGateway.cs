using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class ActionCategoryGateway: ShopBaseGateway
    {
        protected override string TableName => "Action_Category";
        public void Insert(ActionCategory rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(ActionCategory rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public ICollection<ActionCategory> SelectByAction(int actionId)
        {
            return ExecSelect<ActionCategory>(GetProcedureString(), actionId);
        }

        public ICollection<ActionCategory> SelectByCategory(int categoryId)
        {
            return ExecSelect<ActionCategory>(GetProcedureString(), categoryId);
        }
    }
}
