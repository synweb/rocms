using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract;
using RoCMS.Shop.Export.Contract;
using RoCMS.Shop.Export.Contract.Models;

namespace RoCMS.Shop.Web.ApiControllers
{
    [AuthorizeResourcesApi(ShopRoCmsResources.Shop)]
    public class ExportYmlApiController : ApiController
    {
        private IExportShopService _exportShopService;

        public ExportYmlApiController(IExportShopService exportShopService)
        {
            _exportShopService = exportShopService;
        }

        [HttpPost]
        public ResultModel Generate(YmlExportSettings settings)
        {
            _exportShopService.UpdateYmlExportSettings(settings);
            ExportTask task = _exportShopService.StartYmlExportTask(settings);
            return new ResultModel(true, task);
        }

        [HttpGet]
        public List<ExportTask> Tasks()
        {
            return _exportShopService.GetYmlTasks(10);
        }

        [HttpGet]
        public YmlExportSettings Settings()
        {
            return _exportShopService.GetYmlExportSettings();
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetYmlFile()
        {

            string content = _exportShopService.GetYmlFileContent();
            return new HttpResponseMessage() { Content = new StringContent(content, Encoding.UTF8, "application/xml") };
        }
    
    }
}
