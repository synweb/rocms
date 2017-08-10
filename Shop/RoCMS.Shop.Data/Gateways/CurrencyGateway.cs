using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class CurrencyGateway: ShopBaseGateway
    {
        public void Insert(Currency rec)
        {
            Exec(GetProcedureString(), rec);
        }
        public void Update(Currency rec)
        {
            Exec(GetProcedureString(), rec);
        }

        public void Delete(string currencyId)
        {
            Exec(GetProcedureString(), currencyId);
        }

        public Currency SelectOne(string currencyId)
        {
            return Exec<Currency>(GetProcedureString(), currencyId);
        }

        public ICollection<Currency> Select()
        {
            return ExecSelect<Currency>(GetProcedureString());
        }
    }
}
