using System.Collections.Generic;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Models;
using RoCMS.Web.Contract.Models.Shop;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers.Shop
{
    [AuthorizeResources(RoCmsResources.Shop)]
    public class DimensionApiController : ApiController
    {
        private IShopService _shopService;

        public DimensionApiController(IShopService shopService)
        {
            _shopService = shopService;
        }

        public IList<Dimension> GetDimensions()
        {
            return _shopService.GetDimensions();
        }
    }
}
