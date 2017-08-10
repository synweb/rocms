using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCms.Shop.Export.Contract;
using RoCms.Shop.Export.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers.Shop
{
    [AuthorizeResources(RoCmsResources.Shop)]
    public class ExportYmlApiController : ApiController
    {
        private ISettingsService _settingsService;
        private IExportShopService _exportShopService;

        public ExportYmlApiController(ISettingsService settingsService, IExportShopService exportShopService)
        {
            _settingsService = settingsService;
            _exportShopService = exportShopService;
        }

        [HttpPost]
        public ResultModel Generate(YmlExportSettings settings)
        {
            _settingsService.UpdateYmlExportSettings(settings);
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
            return _settingsService.GetYmlExportSettings();
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
