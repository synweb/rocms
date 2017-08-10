using System;
using RoCMS.Web.Contract.Models.Analytics;

namespace RoCMS.Web.Contract.Services
{
    public interface IAnalyticsService
    {
        TrafficSummaryContainer GetTrafficSummary(DateTime? startDate = null, DateTime? endDate = null);
        SourcesSummary GetSourcesSummary(DateTime? startDate = null, DateTime? endDate = null);
    }
}
