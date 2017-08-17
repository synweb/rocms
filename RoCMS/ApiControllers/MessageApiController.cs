using System;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [AllowAnonymous]
    public class MessageApiController : ApiController
    {
        private readonly IFormRequestService _formRequestService;
        private readonly ILogService _logService;

        public MessageApiController(IFormRequestService formRequestService, ILogService logService)
        {
            _formRequestService = formRequestService;
            _logService = logService;
        }

        [HttpPost]
        public ResultModel Order(Message message)
        {
            try
            {
                message.MessageType = MessageType.Order;
                _formRequestService.ProcessMessage(message);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Callback(Message message)
        {
            try
            {
                message.MessageType = MessageType.CallMeBack;
                _formRequestService.ProcessMessage(message);
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
