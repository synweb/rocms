using System;
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
        private readonly ITelegramBotService _telegramBotService;
        private readonly ILogService _logService;

        public TelegramBotApiController(ILogService logService, ITelegramBotService telegramBotService)
        {
            _logService = logService;
            _telegramBotService = telegramBotService;
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