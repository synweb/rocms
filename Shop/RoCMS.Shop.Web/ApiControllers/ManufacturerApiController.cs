using System.Collections.Generic;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    [AuthorizeResourcesApi(ShopRoCmsResources.Shop)]
    public class ManufacturerApiController : ApiController
    {
        private readonly IShopManufacturerService _shopManufacturerService;
        private readonly IShopService _shopService;

        public ManufacturerApiController(IShopManufacturerService shopManufacturerService, IShopService shopService)
        {
            _shopManufacturerService = shopManufacturerService;
            _shopService = shopService;
        }

        [HttpGet]
        public Manufacturer Get(int manufacturerId)
        {
            return _shopManufacturerService.GetManufacturer(manufacturerId);
        }

        [HttpGet]
        public IList<Manufacturer> GetUsedManufacturers()
        {
            return _shopManufacturerService.GetUsedManufacturers();
        }

        [HttpGet]
        public IList<Manufacturer> GetSuppliers()
        {
            return _shopManufacturerService.GetSuppliers();
        }

        [HttpPost]
        public ResultModel Create(Manufacturer manufacturer)
        {
            int id = _shopManufacturerService.CreateManufacturer(manufacturer);
            return new ResultModel(true, new {id = id});
        }

        [HttpPost]
        public ResultModel Update(Manufacturer manufacturer)
        {
            _shopManufacturerService.UpdateManufacturer(manufacturer);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int manufacturerId)
        {
            _shopManufacturerService.DeleteManufacturer(manufacturerId);
            return ResultModel.Success;
        }

        [HttpGet]
        public IList<Manufacturer> GetManufacturers()
        {
            return _shopManufacturerService.GetManufacturers();
        }

        [HttpGet]
        public IEnumerable<Country> GetCountries()
        {
            return _shopService.GetCountries();
        }
    }
}
