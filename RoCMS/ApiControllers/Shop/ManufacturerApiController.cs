using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models.Shop;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers.Shop
{
    [AuthorizeResources(RoCmsResources.Shop)]
    public class ManufacturerApiController : ApiController
    {
        private readonly IShopService _shopService;
        public ManufacturerApiController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public Manufacturer Get(int manufacturerId)
        {
            return _shopService.GetManufacturer(manufacturerId);
        }

        [HttpPost]
        public ResultModel Create(Manufacturer manufacturer)
        {
            int id = _shopService.CreateManufacturer(manufacturer);
            return new ResultModel(true, new {id = id});
        }

        [HttpPost]
        public ResultModel Update(Manufacturer manufacturer)
        {
            _shopService.UpdateManufacturer(manufacturer);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int manufacturerId)
        {
            _shopService.DeleteManufacturer(manufacturerId);
            return ResultModel.Success;
        }

        [HttpGet]
        public IList<Manufacturer> GetManufacturers()
        {
            return _shopService.GetManufacturers();
        }

        [HttpGet]
        public IEnumerable<Country> GetCountries()
        {
            return _shopService.GetCountries();
        }
    }
}
