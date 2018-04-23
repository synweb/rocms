using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Base.Models;
using RoCMS.Data.Gateways;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Services
{
    public class ShopManufacturerService: BaseShopService, IShopManufacturerService
    {
        private readonly ManufacturerGateway _manufacturerGateway = new ManufacturerGateway();
        private readonly CountryGateway _countryGateway = new CountryGateway();

        private readonly IHeartService _heartService;

        public ShopManufacturerService(IHeartService heartService)
        {
            _heartService = heartService;
        }

        public Manufacturer GetManufacturer(int manufacturerId)
        {
            var dataRes = _manufacturerGateway.SelectOne(manufacturerId);

            if (dataRes == null)
            {
                return null;
            }

            var res = Mapper.Map<Manufacturer>(dataRes);
            FillData(res);
            return res;
        }

        private void FillData(Manufacturer manufacturer)
        {
            var heart = _heartService.GetHeart(manufacturer.HeartId);
            manufacturer.FillHeart(heart);

            FillCountry(manufacturer);
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
            manufacturer.Type = manufacturer.GetType().FullName;

            using (TransactionScope ts = new TransactionScope())
            {
                var dataRec = Mapper.Map<Data.Models.Manufacturer>(manufacturer);
                dataRec.Guid = Guid.NewGuid();

                int id = manufacturer.HeartId = dataRec.HeartId = _heartService.CreateHeart(manufacturer);

                _manufacturerGateway.Insert(dataRec);

                ts.Complete();
                return id;
            }
            
        }

        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                _heartService.UpdateHeart(manufacturer);

                var dataRec = Mapper.Map<Data.Models.Manufacturer>(manufacturer);
                _manufacturerGateway.Update(dataRec);
            }
        }

        public void DeleteManufacturer(int manufacturerId)
        {
            _heartService.DeleteHeart(manufacturerId);
        }

        public IList<Manufacturer> GetManufacturers()
        {
            var dataRes = _manufacturerGateway.Select();
            var res = Mapper.Map<IList<Manufacturer>>(dataRes);
            foreach (var manufacturer in res)
            {
                FillData(manufacturer);
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
                FillData(manufacturer);
            }
            return res;
        }

        public IList<Manufacturer> GetUsedManufacturers()
        {
            var dataRes = _manufacturerGateway.SelectUsed();
            var res = Mapper.Map<IList<Manufacturer>>(dataRes);
            foreach (var manufacturer in res)
            {
                FillData(manufacturer);
            }
            return res;
        }
    }
}
