using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using System.Web.UI;
using Newtonsoft.Json;
using RoCMS.Base.ForWeb.Extensions;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Extensions;
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
                return ResultModel.Error;
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
                return ResultModel.Error;
            }
        }

        //[HttpPost]
        //public ResultModel Question(Message message)
        //{
        //    message.MessageType = MessageType.Question;
        //    _formRequestService.ProcessMessage(message);
        //    return ResultModel.Success;
        //}
    }
}
