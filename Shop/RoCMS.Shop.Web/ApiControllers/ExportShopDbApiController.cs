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
    [AuthorizeResources(ShopRoCmsResources.Shop)]
    public class ExportShopDbApiController : ApiController
    {
        private readonly IShopDbExportService _dbExportService;

        public ExportShopDbApiController(IShopDbExportService dbExportService)
        {
            _dbExportService = dbExportService;
        }


        [HttpPost]
        public ResultModel Start()
        {
            var res = _dbExportService.StartDbExportTask();
            return ResultModel.Success;

        }

        [HttpGet]
        public List<DbExportTask> Tasks()
        {
            var res = _dbExportService.GetDbExportTasks(10);
            return res;
        }

    }
}