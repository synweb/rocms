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
    public class CompatibleApiController : ApiController
    {
        private readonly IShopCompatiblesService _shopCompatiblesService;
        public CompatibleApiController(IShopCompatiblesService shopCompatiblesService)
        {
            _shopCompatiblesService = shopCompatiblesService;
        }

        [HttpGet]
        public CompatibleSet Get(int compatibleSetId)
        {
            return _shopCompatiblesService.GetCompatibleSet(compatibleSetId);
        }

        [HttpPost]
        public ResultModel Create(CompatibleSet compatibleSet)
        {
            int id = _shopCompatiblesService.CreateCompatibleSet(compatibleSet);
            return new ResultModel(true, new {id = id});
        }

        [HttpPost]
        public ResultModel Update(CompatibleSet compatibleSet)
        {
            _shopCompatiblesService.UpdateCompatibleSet(compatibleSet);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int compatibleSetId)
        {
            _shopCompatiblesService.DeleteCompatibleSet(compatibleSetId);
            return ResultModel.Success;
        }

        [HttpGet]
        public IList<CompatibleSet> GetCompatibleSets()
        {
            return _shopCompatiblesService.GetCompatibleSets();
        }
    }
}
