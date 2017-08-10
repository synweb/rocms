using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class ActionManufacturerGateway: ShopBaseGateway
    {
        protected override string TableName => "Action_Manufacturer";
        public void Insert(ActionManufacturer rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Delete(ActionManufacturer rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public ICollection<ActionManufacturer> SelectByAction(int actionId)
        {
            return ExecSelect<ActionManufacturer>(GetProcedureString(), actionId);
        }
        public ICollection<ActionManufacturer> SelectByManufacturer(int manufacturerId)
        {
            return ExecSelect<ActionManufacturer>(GetProcedureString(), manufacturerId);
        }
    }
}
