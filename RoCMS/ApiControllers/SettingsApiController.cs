using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

using Newtonsoft.Json;
using Resources;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.CommonSettings)]
    public class SettingsApiController : ApiController
    {
        private readonly ISettingsService _settingsService;
        private readonly ILogService _logService;

        public SettingsApiController(ISettingsService settingsService, ILogService logService)
        {
            _settingsService = settingsService;
            _logService = logService;
        }

        private void UpdateAnalyticsAuthCode(string token, long expires)
        {
            _settingsService.SetAnalyticsAuthToken(token, expires);
        }

        [HttpPost]
        public ResultModel RequestYandexOAuth([FromBody] int code)
        {
            try
            {
                string url = "https://oauth.yandex.ru/token";
                string appId = "12876332b1104b6695e3c211145a3cd6";
                string appPassword = "11e0689175bc424ca0dc14bdae132e3c";
                using (var handler = new HttpClientHandler())
                {
                    HttpClient client = new HttpClient(handler);
                    var data = new Dictionary<string, string>
                    {
                        {"grant_type", "authorization_code"},
                        {"code", code.ToString(CultureInfo.InvariantCulture)},
                        {"client_id", appId},
                        {"client_secret", appPassword},
                    };

                    var content = new FormUrlEncodedContent(data);
                    HttpResponseMessage response =
                        client.PostAsync(url, content).Result;
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        AuthResponse authData = JsonConvert.DeserializeObject<AuthResponse>(responseStr);
                        UpdateAnalyticsAuthCode(authData.access_token, authData.expires_in);
                    }
                    catch
                    {
                        return new ResultModel(false, Strings.AdminAnalytics_BadAuthCode);
                    }
                }
                return new ResultModel(true, new {expiration = _settingsService.GetAnalyticsAuthKeyExpirationDate()});
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        class AuthResponse
        {
            public string access_token { get; set; }
            public long expires_in { get; set; }
        }
    }
}
