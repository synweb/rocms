using System;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.Emails)]
    public class EmailTemplateApiController:ApiController
    {
        private readonly ISettingsService _settingsService;

        public EmailTemplateApiController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpPost]
        public ResultModel UpdateTemplate(UpdateTemplateData data, string name)
        {
            try
            {
                _settingsService.Set("MailTmpl" + name, data.Html);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        public class UpdateTemplateData
        {
            public string Html { get; set; }
        }
    }


    
}