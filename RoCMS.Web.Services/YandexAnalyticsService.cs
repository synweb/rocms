using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RoCMS.Base.Exceptions;
using RoCMS.Web.Contract.Extensions;
using RoCMS.Web.Contract.Models.Analytics;
using RoCMS.Web.Contract.Resources;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class YandexAnalyticsService : IAnalyticsService
    {
        private readonly ISettingsService _settingsService;

        public YandexAnalyticsService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        private const string DATETIME_FORMAT = "yyyyMMdd";

        public TrafficSummaryContainer GetTrafficSummary(DateTime? startDate = null, DateTime? endDate = null)
        {
            var today = DateTime.UtcNow.ApplySiteTimezone().Date;
            if (endDate == null)
                endDate = today;
            if (startDate == null)
                startDate = endDate.Value.AddMonths(-1);
            var res = new TrafficSummaryContainer()
            {
                Id = 0,
                StartDate = startDate.Value,
                EndDate = endDate.Value
            };

            int yaId = _settingsService.GetSettings().YaMetrika;
            string token = _settingsService.GetAnalyticsAuthKey();
            for (DateTime i = startDate.Value; i <= endDate.Value; i = i.AddDays(1))
            {
                res.TrafficSummaryCollection.Add(new TrafficSummary() { EndDate = i });
            }


            var usersResponse = GetAnalyticsResponse(
                $"https://api-metrika.yandex.ru/analytics/v3/data/ga?" +
                $"end-date={endDate.Value.ToString(DATETIME_FORMAT)}" +
                $"&start-date={startDate.Value.ToString(DATETIME_FORMAT)}" +
                $"&ids=ga:{yaId}" +
                $"&metrics=ga:users,ga:sessions,ga:pageviews,ga:newUsers,ga:bounceRate,ga:pageviewsPerSession,ga:avgSessionDuration" +
                $"&dimensions=ga:date" +
                $"&oauth_token={token}");
            foreach (var row in usersResponse.rows)
            {
                var date = DateTime.ParseExact(row[0], DATETIME_FORMAT, CultureInfo.InvariantCulture);
                var dateSummary = res.TrafficSummaryCollection.Single(x => x.EndDate.Equals(date));
                dateSummary.Visitors = int.Parse(row[1]);
                dateSummary.Visits = int.Parse(row[2]);
                dateSummary.PageViews = int.Parse(row[3]);
                dateSummary.NewVisitors = int.Parse(row[4]);
                dateSummary.Denial = float.Parse(row[5], System.Globalization.CultureInfo.InvariantCulture);
                dateSummary.Depth = float.Parse(row[6], System.Globalization.CultureInfo.InvariantCulture);
                dateSummary.VisitTime =
                    TimeSpan.FromSeconds(float.Parse(row[7], System.Globalization.CultureInfo.InvariantCulture));
            }

            return res;
        }

        private AnalyticsResponse GetAnalyticsResponse(string url)
        {

            using (var handler = new HttpClientHandler())
            {
                HttpResponseMessage response;
                using (HttpClient client = new HttpClient(handler))
                {
                    response = client.GetAsync(url).Result;
                }
                string responseStr = response.Content.ReadAsStringAsync().Result;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        throw new OAuthException(Strings.Analytics_Forbidden);
                    case HttpStatusCode.NotFound:
                        throw new UrlNotFoundException(Strings.Analytics_NotFound);
                }
                AnalyticsResponse analyticsResponse = JsonConvert.DeserializeObject<AnalyticsResponse>(responseStr);
                return analyticsResponse;
            }
        }

        public class AnalyticsResponse
        {
            public string kind { get; set; }
            public string id { get; set; }
            public string selfLink { get; set; }
            public bool containsSampledData { get; set; }
            public int sampleSize { get; set; }
            public int sampleSpace { get; set; }
            public int itemsPerPage { get; set; }
            public int totalResults { get; set; }
            public string[][] rows { get; set; }
        }

        public class AnalyticsQuery
        {
            public string ids { get; set; }
            public string[] dimensions { get; set; }
            public string[] metrics { get; set; }
            public string[] sort { get; set; }
            [JsonProperty(PropertyName = "start-date")]
            public DateTime startDate { get; set; }
            [JsonProperty(PropertyName = "end-date")]
            public DateTime endDate { get; set; }
            [JsonProperty(PropertyName = "start-index")]
            public int startIndex { get; set; }
            [JsonProperty(PropertyName = "max-results")]
            public int maxResults { get; set; }

        }

        public SourcesSummary GetSourcesSummary(DateTime? startDate = null, DateTime? endDate = null)
        {

            var today = DateTime.UtcNow.ApplySiteTimezone().Date;
            if (endDate == null)
                endDate = today;
            if (startDate == null)
                startDate = endDate.Value.AddMonths(-1);

            int yaId = _settingsService.GetSettings().YaMetrika;
            string token = _settingsService.GetAnalyticsAuthKey();

            var sourcesResponse = GetAnalyticsResponse(
                $"https://api-metrika.yandex.ru/analytics/v3/data/ga?" +
                $"end-date={endDate.Value.ToString(DATETIME_FORMAT)}" +
                $"&start-date={startDate.Value.ToString(DATETIME_FORMAT)}" +
                $"&ids=ga:{yaId}" +
                $"&metrics=ga:users" +
                $"&dimensions=ga:medium" +
                $"&oauth_token={token}");

            var res = new SourcesSummary();
            var sourceValues = new Dictionary<string, int>();
            foreach (var row in sourcesResponse.rows)
            {
                var value = int.Parse(row[1]);
                sourceValues.Add(row[0], value);
            }
            res.Search = sourceValues.ContainsKey("organic") ? sourceValues["organic"] : 0;
            res.Mail = sourceValues.ContainsKey("email") ? sourceValues["email"] : 0;
            res.Links = sourceValues.ContainsKey("referral") ? sourceValues["referral"] : 0;
            res.Direct = sourceValues.ContainsKey("(none)") ? sourceValues["(none)"] : 0;
            return res;
        }
    }
}
