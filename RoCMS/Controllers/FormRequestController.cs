using System;
using System.Web.Mvc;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Controllers
{

    [AllowAnonymous]
    public class FormRequestController : Controller
    {
        private IFormRequestService _formRequestService;

        public FormRequestController(IFormRequestService formRequestService)
        {
            _formRequestService = formRequestService;
        }

        public ActionResult PaymentAccepted(string id)
        {
            Guid guid;
            bool parsed = Guid.TryParse(id, out guid);
            if (!parsed)
            {
                return new HttpStatusCodeResult(403) {  };
            }
            var formRequestId = _formRequestService.GetOneFormRequest(guid).FormRequestId;
            _formRequestService.UpdatePaymentState(formRequestId, PaymentState.Paid);
            return new RedirectResult("/");
        }
    }
}
