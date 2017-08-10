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
    public class PackApiController : ApiController
    {
        private readonly IShopService _shopService;
        public PackApiController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public Pack Get(int packId)
        {
            return _shopService.GetPack(packId);
        }

        [HttpPost]
        public ResultModel Create(Pack pack)
        {
            int id = _shopService.CreatePack(pack);
            return new ResultModel(true, new {id = id});
        }

        [HttpPost]
        public ResultModel Update(Pack pack)
        {
            _shopService.UpdatePack(pack);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int packId)
        {
            _shopService.DeletePack(packId);
            return ResultModel.Success;
        }

        [HttpGet]
        public IList<Pack> GetPacks()
        {
            return _shopService.GetPacks();
        }
    }
}
