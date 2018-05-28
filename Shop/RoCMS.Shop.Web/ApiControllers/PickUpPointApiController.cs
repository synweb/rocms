using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    public class PickUpPointApiController : ApiController
    {
        private readonly IShopPickupPointService _shopPickupPointService;

        public PickUpPointApiController(IShopPickupPointService shopPickupPointService)
        {
            _shopPickupPointService = shopPickupPointService;
        }

        [HttpGet]
        public IList<PickupPointInfo> GetPoints()
        {
            return _shopPickupPointService.GetPickupPoints().OrderBy(x => x.Title).ToList();
        }

        [HttpPost]
        public ResultModel Create(PickupPointInfo point)
        {
            int id = _shopPickupPointService.CreatePickupPoint(point);
            return new ResultModel(true, id);
        }

        [HttpPost]
        public ResultModel Update(PickupPointInfo point)
        {
            _shopPickupPointService.UpdatePickupPoint(point);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int id)
        {
            _shopPickupPointService.DeletePickupPoint(id);
            return ResultModel.Success;
        }

    }
}
