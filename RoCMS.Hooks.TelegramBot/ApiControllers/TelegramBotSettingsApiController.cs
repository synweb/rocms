using System;
using System.Web;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Hooks.TelegramBot.Models;
using RoCMS.Hooks.TelegramBot.Services;
using RoCMS.Web.Contract.Services;
using Telegram.Bot.Types;

namespace RoCMS.Hooks.TelegramBot.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.AdminPanel)]
    public class TelegramBotSettingsApiController : ApiController
    {

        private readonly ISecurityService _securityService;
        private readonly ITelegramBotService _telegramBotService;
        private readonly ISettingsService _settingsService;
        private readonly ILogService _logService;

        public TelegramBotSettingsApiController(ISecurityService securityService, ISearchService searchService, ILogService logService, ITelegramBotService telegramBotService, ISettingsService settingsService)
        {
            _securityService = securityService;
            _logService = logService;
            _telegramBotService = telegramBotService;
            _settingsService = settingsService;
        }

        [HttpPost]
        public ResultModel Update(TelegramBotSettings settings)
        {
            try
            {
                _telegramBotService.UpdateSettings(settings);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return ResultModel.Error;
            }
        }

        [HttpGet]
        public TelegramBotSettings Get()
        {
            return _telegramBotService.GetSettings();
        }
    }
}