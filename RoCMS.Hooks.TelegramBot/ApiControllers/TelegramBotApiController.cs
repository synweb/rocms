//using System;
//using System.Web;
//using System.Web.Http;

//using RoCMS.Hooks.TelegramBot.Services;
//using RoCMS.Web.Contract.Services;

//namespace RoCMS.Hooks.TelegramBot.ApiControllers
//{

//    public class TelegramBotApiController : ApiController
//    {
        
//        private readonly ISecurityService _securityService;
//        private readonly ITelegramBotService _telegramBotService;

//        private readonly ILogService _logService;
        
//        public TelegramBotApiController(ISecurityService securityService, ISearchService searchService, ILogService logService, ITelegramBotService telegramBotService)
//        {
//            _securityService = securityService;
//            _logService = logService;
//            _telegramBotService = telegramBotService;
//        }

//        [HttpPost]
//        public void ReceiveWebHook()
//        {
//            _telegramBotService.ReceiveMessage();
//        }


//    }
//}