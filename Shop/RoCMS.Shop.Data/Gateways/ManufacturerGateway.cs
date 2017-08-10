using System.Collections.Generic;
using RoCMS.Data.Models;
using Manufacturer = RoCMS.Shop.Data.Models.Manufacturer;

namespace RoCMS.Shop.Data.Gateways
{
    public class ManufacturerGateway: ShopBasicGateway<Models.Manufacturer>
    {
        public ICollection<Country> SelectManufacturerCountries()
        {
            //TODO: проверить, может не работать
            return ExecSelect<Country>(GetProcedureString());
        }

        public ICollection<Manufacturer> SelectUsed()
        {
            return ExecSelect<Manufacturer>(GetProcedureString());
        }

        public ICollection<Manufacturer> SelectSuppliers()
        {
            return ExecSelect<Manufacturer>(GetProcedureString());
        }
    }
}
