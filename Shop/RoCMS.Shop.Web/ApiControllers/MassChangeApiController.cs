using System;
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
    public class MassChangeApiController : ApiController
    {
        private IMassChangeService _massChangeService;

        public MassChangeApiController(IMassChangeService massChangeService)
        {
            _massChangeService = massChangeService;
        }

        [HttpPost]
        public ResultModel ChangePrice(MassPriceChange change)
        {
            try
            {
                var task = _massChangeService.StartChangePriceTask(change);
                return new ResultModel(true, task);
            }
            catch (Exception e)
            {
                return new ResultModel(false, e.Message);
            }
        }

        public IEnumerable<MassPriceChangeTask> GetChangePriceTasks()
        {
            return _massChangeService.GetChangePriceTasks();
        }
    }
}
