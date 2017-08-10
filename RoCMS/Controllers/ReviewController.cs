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
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        
        [AllowAnonymous]
        public ActionResult GetModeratedReviewPage(int startIndex = 1, int pageSize = 10)
        {
            //int total;
            //var reviews = _reviewService.GetModeratedReviewPage(start, pageSize, out total);
            ViewBag.StartIndex = startIndex;
            ViewBag.PageSize = pageSize;
            return PartialView("_ReviewList");
        }
        
        [AllowAnonymous]
        public ActionResult GetReview(int id)
        {
            var review = _reviewService.GetReview(id);
            return PartialView(review);
        }

        [AllowAnonymous]
        public ActionResult CreateReview()
        {
            ViewBag.Action = "CreateReview";
            return PartialView("_ReviewEditor");
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
            _reviewService.CreateReview(review);
            return Json(new ResultModel(true));
        }

       


    }
}
