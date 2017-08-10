using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class SpecCategoryGateway:ShopBaseGateway
    {
        protected override string TableName => "Spec_Category";
        public void Insert(SpecCategory rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(SpecCategory rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public ICollection<SpecCategory> SelectByCategory(int categoryId)
        {
            return ExecSelect<SpecCategory>(GetProcedureString(), categoryId);
        }
    }
}
