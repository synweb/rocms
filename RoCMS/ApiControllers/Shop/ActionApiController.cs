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

using Action = RoCMS.Web.Contract.Models.Shop.Action;

namespace RoCMS.ApiControllers.Shop
{
    [AuthorizeResources(RoCmsResources.Shop)]
    public class ActionApiController : ApiController
    {
        private readonly IShopService _shopService;

        public ActionApiController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public IList<Action> GetActions()
        {
            return _shopService.GetActions();
        }

        [HttpGet]
        public Action Get(int actionId)
        {
            return _shopService.GetAction(actionId);
        }

        [HttpPost]
        public ResultModel Create(Action action)
        {
            int id = _shopService.CreateAction(action);
            return new ResultModel(true, new { id = id });
        }

        [HttpPost]
        public ResultModel Update(Action action)
        {
            _shopService.UpdateAction(action);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int actionId)
        {
            _shopService.DeleteAction(actionId);
            return ResultModel.Success;
        }
    }
}
