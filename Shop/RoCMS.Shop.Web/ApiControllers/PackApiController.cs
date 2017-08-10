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
    public class PackApiController : ApiController
    {
        private readonly IShopPackService _shopPackService;
        public PackApiController(IShopPackService shopPackService)
        {
            _shopPackService = shopPackService;
        }

        [HttpGet]
        public Pack Get(int packId)
        {
            return _shopPackService.GetPack(packId);
        }

        [HttpPost]
        public ResultModel Create(Pack pack)
        {
            int id = _shopPackService.CreatePack(pack);
            return new ResultModel(true, new {id = id});
        }

        [HttpPost]
        public ResultModel Update(Pack pack)
        {
            _shopPackService.UpdatePack(pack);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int packId)
        {
            _shopPackService.DeletePack(packId);
            return ResultModel.Success;
        }

        [HttpGet]
        public IList<Pack> GetPacks()
        {
            return _shopPackService.GetPacks();
        }
    }
}
