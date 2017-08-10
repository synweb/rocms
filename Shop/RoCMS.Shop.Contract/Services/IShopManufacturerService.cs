using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopManufacturerService
    {

        Manufacturer GetManufacturer(int manufacturerId);
        int CreateManufacturer(Manufacturer manufacturer);
        void UpdateManufacturer(Manufacturer manufacturer);
        void DeleteManufacturer(int manufacturerId);
        IList<Manufacturer> GetManufacturers();
        /// <summary>
        /// Возвращает страны всех производителей
        /// </summary>
        /// <returns></returns>
        IEnumerable<Country> GetManufactuterCountries();

        IList<Manufacturer> GetSuppliers();
        IList<Manufacturer> GetUsedManufacturers();
    }
}
