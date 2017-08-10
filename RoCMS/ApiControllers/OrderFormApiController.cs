using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [System.Web.Http.Authorize]
    [AuthorizeResourcesApi(RoCmsResources.CommonSettings)]
    public class OrderFormApiController : ApiController
    {
        private IOrderFormService _orderFormService;
        private ILogService _logService;

        public OrderFormApiController(IOrderFormService orderFormService, ILogService logService)
        {
            _orderFormService = orderFormService;
            _logService = logService;
        }

        [HttpGet]
        public ResultModel GetForm(int formId)
        {
            try
            {
               var form = _orderFormService.GetOrderForm(formId);
                return new ResultModel(true, form);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return ResultModel.Error;
            }
        }

        [HttpPost]
        public ResultModel CreateForm(OrderForm form)
        {
            try
            {
                int id = _orderFormService.CreateOrderForm(form);
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return ResultModel.Error;
            }
        }

        [HttpPost]
        public ResultModel UpdateForm(OrderForm form)
        {
            try
            {
                _orderFormService.SaveOrderForm(form);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return ResultModel.Error;
            }
        }

        [HttpPost]
        public ResultModel DeleteForm(int formId)
        {
            try
            {
                _orderFormService.DeleteOrderForm(formId);
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