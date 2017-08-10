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
using RoCMS.Web.Contract.Services.Shop;

namespace RoCMS.ApiControllers.Shop
{
    [AuthorizeResources(RoCmsResources.Shop)]
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
