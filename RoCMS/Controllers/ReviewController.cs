using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly ILogService _logService;

        public ReviewController(IReviewService reviewService, ILogService logService)
        {
            _reviewService = reviewService;
            _logService = logService;
        }
        
        [AllowAnonymous]
        [PagingFilter]
        public ActionResult Page(int pageSize, int pageNumber)
        {
            int total;
            var reviews = _reviewService.GetModeratedReviewPage((pageSize * (pageNumber - 1) +1), pageSize, out total);

            ViewBag.TotalCount = total;
            return PartialView("_ReviewList", reviews);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreateReview(Review review)
        {
            try
            {
                _reviewService.CreateReview(review);
                return Json(ResultModel.Success);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return Json(new ResultModel(e));
            }
        }
    }
}
