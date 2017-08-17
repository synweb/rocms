using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Resources;

namespace RoCMS.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.Emails)]
    public class MailApiController : ApiController
    {
        private readonly IMailService _mailService;
        private readonly ILogService _logService;

        public MailApiController(IMailService mailService, ILogService logService)
        {
            _mailService = mailService;
            _logService = logService;
        }

        [HttpPost]
        public ResultModel ReSendMail(int mailId)
        {
            try
            {
                var sendResult = _mailService.ReSendMessage(mailId);
                return new ResultModel(sendResult.Success, sendResult.ErrorMsg,
                    sendResult.Success ? Strings.AdminMail_Sent : Strings.AdminMail_NotSent);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel DeleteMail(int mailId)
        {
            try
            {
                _mailService.DeleteMessage(mailId);
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