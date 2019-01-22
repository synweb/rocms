﻿using System;
using System.Web;
using System.Web.Http;

using RoCMS.Hooks.TelegramBot.Services;
using RoCMS.Web.Contract.Services;
using Telegram.Bot.Types;

namespace RoCMS.Hooks.TelegramBot.ApiControllers
{
    [AllowAnonymous]
    public class TelegramBotApiController : ApiController
    {

        private readonly ISecurityService _securityService;
        private readonly ITelegramBotService _telegramBotService;
        private readonly ISettingsService _settingsService;
        private readonly ILogService _logService;

        public TelegramBotApiController(ISecurityService securityService, ISearchService searchService, ILogService logService, ITelegramBotService telegramBotService, ISettingsService settingsService)
        {
            _securityService = securityService;
            _logService = logService;
            _telegramBotService = telegramBotService;
            _settingsService = settingsService;
        }

        [HttpPost]
        public void ReceiveWebHook([FromBody] Update model)
        {
            try
            {
                _telegramBotService.ReceiveMessage(model);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
            }
        }


    }
}