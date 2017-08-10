using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class ActionGateway: ShopBasicGateway<Action>
    {
        public bool Exists(int id)
        {
            return Exec<bool>(GetProcedureString(), id);
        }
    }
}
