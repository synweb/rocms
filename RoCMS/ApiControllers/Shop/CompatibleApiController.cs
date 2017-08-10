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
    public class CompatibleApiController : ApiController
    {
        private readonly IShopService _shopService;
        public CompatibleApiController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public CompatibleSet Get(int compatibleSetId)
        {
            return _shopService.GetCompatibleSet(compatibleSetId);
        }

        [HttpPost]
        public ResultModel Create(CompatibleSet compatibleSet)
        {
            int id = _shopService.CreateCompatibleSet(compatibleSet);
            return new ResultModel(true, new {id = id});
        }

        [HttpPost]
        public ResultModel Update(CompatibleSet compatibleSet)
        {
            _shopService.UpdateCompatibleSet(compatibleSet);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int compatibleSetId)
        {
            _shopService.DeleteCompatibleSet(compatibleSetId);
            return ResultModel.Success;
        }

        [HttpGet]
        public IList<CompatibleSet> GetCompatibleSets()
        {
            return _shopService.GetCompatibleSets();
        }
    }
}
