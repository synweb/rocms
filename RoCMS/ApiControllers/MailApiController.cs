using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RoCMS.ApiControllers
{
    [System.Web.Http.Authorize]
    [AuthorizeResourcesApi(RoCmsResources.Emails)]
    public class MailApiController : ApiController
    {
        private readonly IMailService _mailService;

        public MailApiController(IMailService mailService)
        {            
            _mailService = mailService;
        }

        [HttpPost]
        public ResultModel ReSendMail(int mailId)
        {
            var sendResult = _mailService.ReSendMessage(mailId);
            return new ResultModel(sendResult.Success, sendResult.ErrorMsg,
                sendResult.Success ? Resources.Strings.AdminMail_Sent : Resources.Strings.AdminMail_NotSent);
        }

        [HttpPost]
        public ResultModel DeleteMail(int mailId)
        {
            _mailService.DeleteMessage(mailId);
            return new ResultModel(true);
        }
    }
}