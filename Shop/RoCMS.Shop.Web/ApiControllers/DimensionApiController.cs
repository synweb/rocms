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
    public class DimensionApiController : ApiController
    {
        private readonly IShopPackService _shopPackService;

        public DimensionApiController(IShopPackService shopPackService)
        {
            _shopPackService = shopPackService;
        }

        public IList<Dimension> GetDimensions()
        {
            return _shopPackService.GetDimensions();
        }
    }
}
