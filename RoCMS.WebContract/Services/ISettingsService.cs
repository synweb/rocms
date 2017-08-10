using System;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface ISettingsService
    {
        void UpdateSettings(Setting model);
        void UpdateEmailPassword(string password);
        string GetSiteName();
        string GetHomepageUrl();
        Setting GetSettings();
        int GetMainMenuId();
        void SetAnalyticsAuthToken(string token, long expires);
        DateTime GetAnalyticsAuthKeyExpirationDate();
        string GetAnalyticsAuthKey();
        TimeSpan GetTicketLifetime();
        T GetSettings<T>(string key);
        void Set<T>(string key, T value);
        string[] GetEmailTemplateNames();
    }
}
