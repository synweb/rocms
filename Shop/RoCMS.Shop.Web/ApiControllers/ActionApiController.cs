using System;
using System.Collections.Generic;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract;
using RoCMS.Shop.Contract.Services;
using Action = RoCMS.Shop.Contract.Models.Action;

namespace RoCMS.Shop.Web.ApiControllers
{
    [AuthorizeResourcesApi(ShopRoCmsResources.Shop)]
    public class ActionApiController : ApiController
    {
        private readonly IShopActionService _shopActionService;

        public ActionApiController(IShopActionService shopActionService)
        {
            _shopActionService = shopActionService;
        }

        [HttpGet]
        public IList<Action> GetActions()
        {
            return _shopActionService.GetActions();
        }

        [HttpGet]
        public Action Get(int actionId)
        {
            return _shopActionService.GetAction(actionId);
        }

        [HttpPost]
        public ResultModel Create(Action action)
        {
            int id = _shopActionService.CreateAction(action);
            return new ResultModel(true, new { id = id });
        }

        [HttpPost]
        public ResultModel Update(Action action)
        {
            _shopActionService.UpdateAction(action);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel Delete(int actionId)
        {
            _shopActionService.DeleteAction(actionId);
            return ResultModel.Success;
        }
    }
}
