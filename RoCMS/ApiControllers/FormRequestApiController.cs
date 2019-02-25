using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{

    [AuthorizeResourcesApi(RoCmsResources.Requests)]
    public class FormRequestApiController : ApiController
    {
        private IFormRequestService _formRequestService;
        private readonly ILogService _logService;

        public FormRequestApiController(ILogService logService, IFormRequestService formRequestService)
        {

            _logService = logService;
            _formRequestService = formRequestService;

        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateState(int id, FormRequestState state)
        {
            try
            {
                _formRequestService.UpdateFormRequestState(id, state);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return ResultModel.Error;
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdatePaymentState(int id, PaymentState state)
        {
            try
            {
                _formRequestService.UpdatePaymentState(id, state);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return ResultModel.Error;
            }
        }
    }
}
