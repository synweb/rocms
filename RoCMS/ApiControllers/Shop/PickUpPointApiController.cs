using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models.Shop;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers.Shop
{
    public class PickUpPointApiController : ApiController
    {
        private IShopService _shopService;

        public PickUpPointApiController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public IList<PickupPointInfo> GetPoints()
        {
            return _shopService.GetPickupPoints();
        }

        [HttpPost]
        public ResultModel Create(PickupPointInfo point)
        {
            int id = _shopService.CreatePickupPoint(point);
            return new ResultModel(true, id);
        }

        [HttpPost]
        public ResultModel Update(PickupPointInfo point)
        {
            _shopService.UpdatePickupPoint(point);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int id)
        {
            _shopService.DeletePickupPoint(id);
            return ResultModel.Success;
        }

    }
}
