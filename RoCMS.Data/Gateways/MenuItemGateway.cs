using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class MenuItemGateway:BaseGateway
    {
        public ICollection<MenuItem> Select(int menuId)
        {
            return ExecSelect<MenuItem>(GetProcedureString(), menuId);
        } 

        public ICollection<MenuItem> SelectChildren(int menuItemId)
        {
            return ExecSelect<MenuItem>(GetProcedureString(), menuItemId);
        }

        public void Update(MenuItem dataItem)
        {
            Exec(GetProcedureString(), dataItem);
        }

        public int Insert(MenuItem dataItem)
        {
            return Exec<int>(GetProcedureString(), dataItem);
        }

        public void Delete(int itemId)
        {
            Exec(GetProcedureString(), itemId);
        }
    }
}
