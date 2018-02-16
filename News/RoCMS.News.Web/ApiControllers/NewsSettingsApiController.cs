using System;
using System.Collections.Generic;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Web.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.News)]
    public class NewsSettingsApiController : ApiController
    {
        private readonly INewsSettingsService _settingsService;
        private readonly ILogService _logService;
        private readonly IRssCrawlingService _rssCrawlingService;

        public NewsSettingsApiController(INewsSettingsService settingsService, ILogService logService, IRssCrawlingService rssCrawlingService)
        {
            _settingsService = settingsService;
            _logService = logService;
            _rssCrawlingService = rssCrawlingService;
        }

        [HttpGet]
        public NewsSettings Get()
        {
            return _settingsService.GetNewsSettings();
        }

        [HttpGet]
        public ResultModel GetRssCrawlers()
        {
            return new ResultModel(true, _rssCrawlingService.GetCrawlers());
        }

        public class UpdateRssCrawlersData
        {
            public UpdateRssCrawlersData()
            {
                Crawlers = new List<RssCrawler>();
            }

            public ICollection<RssCrawler> Crawlers { get; set; }
        }

        [HttpPost]
        public ResultModel UpdateRssCrawlers(UpdateRssCrawlersData data)
        {
            try
            {
                _rssCrawlingService.UpdateCrawlers(data.Crawlers);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Update(NewsSettings settings)
        {
            try
            {
                _settingsService.UpdateNewsSettings(settings);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
    }
}
