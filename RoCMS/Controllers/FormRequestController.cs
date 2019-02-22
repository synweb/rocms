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

        public ActionResult PaymentAccepted(int formRequestId)
        {
            _formRequestService.UpdatePaymentState(formRequestId, PaymentState.Paid);

            return RedirectToAction("MainPage", "Page");
        }
    }
}
