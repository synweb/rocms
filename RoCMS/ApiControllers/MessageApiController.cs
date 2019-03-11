using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.Routing;
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
        private readonly IPaymentSystemService _paymentSystemService;
        private readonly ISettingsService _settingsService;

        public MessageApiController(IFormRequestService formRequestService, ILogService logService, IPaymentSystemService paymentSystemService, ISettingsService settingsService)
        {
            _formRequestService = formRequestService;
            _logService = logService;
            _paymentSystemService = paymentSystemService;
            _settingsService = settingsService;
        }

        /// <summary>
        /// /api/message/send/order
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel Order(Message message)
        {
            try
            {
                message.MessageType = MessageType.Order;
                int id = _formRequestService.ProcessMessage(message);

                if (message.PaymentType == PaymentType.Card && message.Amount.HasValue)
                {

                    var settings = _settingsService.GetSettings();
                    string rootUrl = settings.RootUrl;
                    // getting guid
                    var fromRequest = _formRequestService.GetOneFormRequest(id);
                    var returnUrl = $"{rootUrl}/FormRequest/PaymentAccepted/{fromRequest.Guid}";
                    string redirectUrl = _paymentSystemService.ProcessPayment(id, typeof(FormRequest).FullName, message.Amount.Value, returnUrl);
                    return new ResultModel(true, new { RedirectUrl = redirectUrl });
                }

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
