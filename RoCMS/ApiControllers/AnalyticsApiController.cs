using System;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models.Analytics;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [Authorize]
    public class AnalyticsApiController : ApiController
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsApiController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet]
        public ResultModel GetTrafficSummary(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {

                TrafficSummaryContainer res;
                if(!startDate.HasValue || !endDate.HasValue)
                {
                    res = _analyticsService.GetTrafficSummary();
                }
                else
                {
                    res = _analyticsService.GetTrafficSummary(startDate, endDate);
                }
                
                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                return new ResultModel(false, e.Message);
            }
        }

        [HttpGet]
        public ResultModel GetDefaultTrafficSummary()
        {
            return GetTrafficSummary();
        }

        [HttpGet]
        public ResultModel GetSourcesSummary(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                SourcesSummary res;
                if (!startDate.HasValue || !endDate.HasValue)
                {
                    res = _analyticsService.GetSourcesSummary();
                }
                else
                {
                    res = _analyticsService.GetSourcesSummary(startDate, endDate);
                }
                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                return new ResultModel(false, e.Message);
            }
        }

        [HttpGet]
        public ResultModel GetDefaultSourcesSummary()
        {
            return GetSourcesSummary();
        }
    }
}
