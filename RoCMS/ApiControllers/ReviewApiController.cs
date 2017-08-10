using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{

    [AuthorizeResourcesApi(RoCmsResources.Reviews)]
    public class ReviewApiController : ApiController
    {
        private readonly IReviewService _reviewService;

        public ReviewApiController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ResultModel Create(Review review)
        {
            try
            {
                int id = _reviewService.CreateReview(review);
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                return new ResultModel(false, e.Message);
            }
        }

        [AuthorizeResourcesApi(RoCmsResources.Reviews)]
        [HttpPost]
        public ResultModel Update(Review review)
        {
            try
            {
                _reviewService.UpdateReview(review);
                return new ResultModel(true);
            }
            catch (Exception e)
            {
                return new ResultModel(false, e.Message);
            }
        }

        [AuthorizeResourcesApi(RoCmsResources.Reviews)]
        [HttpPost]
        public ResultModel Accept(int id)
        {
            try
            {
                _reviewService.ModerateReview(id, true);
                return new ResultModel(true);
            }
            catch (Exception e)
            {
                return new ResultModel(false, e.Message);
            }
        }

        [AuthorizeResourcesApi(RoCmsResources.Reviews)]
        [HttpPost]
        public ResultModel Hide(int id)
        {
            try
            {
                _reviewService.ModerateReview(id, false);
                return new ResultModel(true);
            }
            catch (Exception e)
            {
                return new ResultModel(false, e.Message);
            }
        }

        [AuthorizeResourcesApi(RoCmsResources.Reviews)]
        [HttpPost]
        public ResultModel Delete(int id)
        {
            try
            {
                _reviewService.DeleteReview(id);
                return new ResultModel(true);
            }
            catch (Exception e)
            {
                return new ResultModel(false, e.Message);
            }
        }
    }
}
