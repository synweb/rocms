using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.Base.Models;
using RoCMS.Data.Gateways;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;

namespace RoCMS.Shop.Services
{
    public class ShopManufacturerService: BaseShopService, IShopManufacturerService
    {
        private readonly ManufacturerGateway _manufacturerGateway = new ManufacturerGateway();
        private readonly CountryGateway _countryGateway = new CountryGateway();
        public Manufacturer GetManufacturer(int manufacturerId)
        {
            var dataRes = _manufacturerGateway.SelectOne(manufacturerId);
            var res = Mapper.Map<Manufacturer>(dataRes);
            FillCountry(res);
            return res;
        }

        private void FillCountry(Manufacturer manufacturer)
        {
            if(manufacturer.CountryId == null)
                return;
            var country = _countryGateway.SelectOne(manufacturer.CountryId.Value);
            manufacturer.Country = new IdNamePair<int>(country.CountryId, country.Name);
        }

        public int CreateManufacturer(Manufacturer manufacturer)
        {
            var dataRec = Mapper.Map<Data.Models.Manufacturer>(manufacturer);
            dataRec.Guid = Guid.NewGuid();
            int id = _manufacturerGateway.Insert(dataRec);
            return id;
        }

        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            var dataRec = Mapper.Map<Data.Models.Manufacturer>(manufacturer);
            _manufacturerGateway.Update(dataRec);
        }

        public void DeleteManufacturer(int manufacturerId)
        {
            _manufacturerGateway.Delete(manufacturerId);
        }

        public IList<Manufacturer> GetManufacturers()
        {
            var dataRes = _manufacturerGateway.Select();
            var res = Mapper.Map<IList<Manufacturer>>(dataRes);
            foreach (var manufacturer in res)
            {
                FillCountry(manufacturer);
            }
            return res;
        }

        public IEnumerable<Country> GetManufactuterCountries()
        {
            var dataRes = _manufacturerGateway.SelectManufacturerCountries();
            var res = Mapper.Map<ICollection<Country>>(dataRes);
            return res;
        }

        public IList<Manufacturer> GetSuppliers()
        {
            var dataRes = _manufacturerGateway.SelectSuppliers();
            var res = Mapper.Map<IList<Manufacturer>>(dataRes);
            foreach (var manufacturer in res)
            {
                FillCountry(manufacturer);
            }
            return res;
        }

        public IList<Manufacturer> GetUsedManufacturers()
        {
            var dataRes = _manufacturerGateway.SelectUsed();
            var res = Mapper.Map<IList<Manufacturer>>(dataRes);
            foreach (var manufacturer in res)
            {
                FillCountry(manufacturer);
            }
            return res;
        }
    }
}
