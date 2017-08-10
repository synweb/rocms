using System.Collections.Generic;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    public class RegularClientApiController : ApiController
    {
        private readonly IShopClientService _shopClientService;

        public RegularClientApiController(IShopClientService shopClientService)
        {
            _shopClientService = shopClientService;
        }


        [HttpGet]
        public IList<RegularClientDiscount> Get()
        {
            return _shopClientService.GetRegularClientDiscounts();
        }

        [HttpPost]
        public ResultModel Create(RegularClientDiscount discount)
        {
            int id = _shopClientService.CreateRegularClientDiscounts(discount);
            return new ResultModel(true, id);
        }

        [HttpPost]
        public ResultModel Update(RegularClientDiscount discount)
        {
            _shopClientService.UpdateRegularClientDiscount(discount);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int id)
        {
            _shopClientService.DeleteRegularClientDiscount(id);
            return ResultModel.Success;
        }
    }
}