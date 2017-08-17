using System;
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

        public NewsSettingsApiController(INewsSettingsService settingsService, ILogService logService)
        {
            _settingsService = settingsService;
            _logService = logService;
        }

        [HttpGet]
        public NewsSettings Get()
        {
            return _settingsService.GetNewsSettings();
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
