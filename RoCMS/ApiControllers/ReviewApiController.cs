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
        private readonly ILogService _logService;
        private readonly ISettingsService _settingsService;

        public ReviewApiController(IReviewService reviewService, ILogService logService, ISettingsService settingsService)
        {
            _reviewService = reviewService;
            _logService = logService;
            _settingsService = settingsService;
        }

        [AllowAnonymous]
        [HttpPost]
        public ResultModel Create(Review review)
        {
            try
            {
                int id = _reviewService.CreateReview(review, _settingsService.GetSettings<bool>(nameof(Setting.ReviewCreatedNotification)));
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
        
        [HttpPost]
        public ResultModel CreateByAdmin(Review review)
        {
            try
            {
                int id = _reviewService.CreateReview(review, false);
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Update(Review review)
        {
            try
            {
                _reviewService.UpdateReview(review);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Accept(int id)
        {
            try
            {
                _reviewService.ModerateReview(id, true);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Hide(int id)
        {
            try
            {
                _reviewService.ModerateReview(id, false);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Delete(int id)
        {
            try
            {
                _reviewService.DeleteReview(id);
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
