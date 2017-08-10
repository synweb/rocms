using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;

namespace RoCMS.News.Web.ApiControllers
{


    public class NewsSettingsApiController : ApiController
    {
        private readonly INewsSettingsService _settingsService;

        public NewsSettingsApiController(INewsSettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        [HttpGet]
        public NewsSettings Get()
        {
            return _settingsService.GetNewsSettings();
        }
        [HttpPost]
        public ResultModel Update(NewsSettings settings)
        {
            _settingsService.UpdateNewsSettings(settings);
            return ResultModel.Success;
        }
    }
}
