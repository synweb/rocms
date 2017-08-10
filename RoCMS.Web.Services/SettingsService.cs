using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Remoting.Contexts;
using System.Web.Mvc;
using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Base.Services;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    //TODO: здесь нужен рефакторинг: кучу методов заменить на универсальные, все строки - в SettingKey
    public class SettingsService : BaseCoreService, ISettingsService
    {
        private readonly SettingGateway _settingGateway = new SettingGateway();

        public SettingsService()
        {
            InitCache("SettingsServiceMemoryCache");
        }

        public T GetSettings<T>(string key)
        {
            string setting = GetSettingString(key);
            if (!String.IsNullOrEmpty(setting))
            {
                T value = Mapper.Map<T>(setting);
                return value;
            }
            return default(T);
        }

        public void Set<T>(string key, T value)
        {
            string setting;
            if (!(value is ValueType) && value == null)
            {
                setting = String.Empty;
            }
            else
            {
                setting = typeof(T) == typeof(string) ? value as string : Mapper.Map<string>(value);
            }
            UpdateOneSetting(key, setting);

        }

        public string[] GetEmailTemplateNames()
        {
            var res = _settingGateway.Select().Where(x => x.Key.StartsWith("MailTmpl")).Select(x => x.Key.Replace("MailTmpl", "")).ToArray();
            return res;
        }

        //public T GetSettings<T>(SettingKey key)
        //{
        //    string keyStr = GetSettingName(key);
        //    string setting = GetSettingString(keyStr);
        //    if (!String.IsNullOrEmpty(setting))
        //    {
        //        T value = Mapper.Map<T>(setting);
        //        return value;
        //    }
        //    return default(T);
        //}

        //public void Set<T>(SettingKey key, T value)
        //{
        //    string settingName = GetSettingName(key);
        //    string setting;
        //    if (!(value is ValueType) && value == null)
        //    {
        //        setting = String.Empty;
        //    }
        //    else
        //    {
        //        setting = typeof (T) == typeof (string) ? value as string : Mapper.Map<string>(value);
        //    }
        //    UpdateOneSetting(settingName, setting);

        //}

        public void UpdateSettings(Setting model)
        {
            ValidateSettings(model);
            UpdateOneSetting("MainMenuId", model.MainMenuId);
            UpdateOneSetting("MainPageUrl", model.MainPageUrl);
            UpdateOneSetting("SiteName", model.SiteName);
            UpdateOneSetting("Reviews", model.Reviews);
            UpdateOneSetting("Timezone", model.Timezone);
            if (model.YaMetrika != 0)
            {
                UpdateOneSetting("YaMetrika", model.YaMetrika);
            }
            else
            {
                RemoveSetting("YaMetrika");
            }

            UpdateOneSetting("RootUrl", model.RootUrl);
            UpdateOneSetting("ImageMaxHeight", model.ImageMaxHeight);
            UpdateOneSetting("ImageMaxWidth", model.ImageMaxWidth);
            UpdateOneSetting("ImageQuality", model.ImageQuality);
            UpdateOneSetting("AutoEmailReplyEnabled", model.AutoEmailReplyEnabled);
            UpdateOneSetting("EmailSmtpUrl", model.EmailSmtpUrl);
            UpdateOneSetting("EmailSmtpPort", model.EmailSmtpPort);
            UpdateOneSetting("SmtpSslEnabled", model.SmtpSslEnabled);
            UpdateOneSetting("EmailLogin", model.EmailLogin);
            UpdateOneSetting("OrderEmailAddress", model.OrderEmailAddress);
            UpdateOneSetting("SystemEmailAddress", model.SystemEmailAddress);
            UpdateOneSetting("SystemEmailSenderName", model.SystemEmailSenderName);
            UpdateOneSetting("TranslitEnabled", model.TranslitEnabled);
            UpdateOneSetting("RootBreadcrumbsTitle", model.RootBreadcrumbsTitle);
            UpdateOneSetting("AllowedFileExtensions", model.AllowedFileExtensions);
            UpdateOneSetting("YoutubeAPIKey", model.YoutubeAPIKey);


            UpdateOneSetting(nameof(Setting.ThumbnailSizes), model.ThumbnailSizes);
            var imageService = DependencyResolver.Current.GetService<IImageService>();
            imageService.ClearUnusedThumbnailDirectories();

        }

        private void ValidateSettings(Setting model)
        {
            if (!ValidationHelper.ValidateThumbnailSizes(model.ThumbnailSizes))
            {
                throw new ValidationException(nameof(model.ThumbnailSizes));
            }
        }

        public void UpdateEmailPassword(string password)
        {
            UpdateOneSetting("EmailPassword", password);
        }

        private void RemoveSetting(string name)
        {
            _settingGateway.Delete(name);
            RemoveObjectFromCache(name);
        }


        public string GetSiteName()
        {
            return GetSettingString("SiteName");
        }

        public string GetHomepageUrl()
        {
            return GetSettingString("MainPageUrl");
        }

        public Setting GetSettings()
        {
            int mainMenuId = GetSettingInt("MainMenuId");
            string mainPageUrl = GetSettingString("MainPageUrl");
            string siteName = GetSettingString("SiteName");
            bool reviews = GetSettingBool("Reviews");
            int yaMetrika = GetSettingInt("YaMetrika");
            int timezone = GetSettingInt("Timezone");
            DateTime? yaMetrikaAuthKeyExpires = null;
            try
            {
                yaMetrikaAuthKeyExpires = GetSettingDateTime("AnalyticsAuthKeyExpires");
            }
            catch
            { }

            string rootUrl = GetSettingString("RootUrl");
            int imageMaxHeight = GetSettingInt("ImageMaxHeight");
            int imageMaxWidth = GetSettingInt("ImageMaxWidth");
            int imageQuality = GetSettingInt("ImageQuality");
            bool autoEmailReplyEnabled = GetSettingBool("AutoEmailReplyEnabled");
            string emailSmtpUrl = GetSettingString("EmailSmtpUrl");
            int emailSmtpPort = GetSettingInt("EmailSmtpPort");
            bool smtpSslEnabled = GetSettingBool("SmtpSslEnabled");
            string emailLogin = GetSettingString("EmailLogin");
            string emailPassword = GetSettingString("EmailPassword");
            string orderEmailAddress = GetSettingString("OrderEmailAddress");
            string systemEmailAddress = GetSettingString("SystemEmailAddress");
            string systemEmailSenderName = GetSettingString("SystemEmailSenderName");
            string rootBreadcrumbsTitle = GetSettingString("RootBreadcrumbsTitle");
            bool translitEnabled = GetSettingBool("TranslitEnabled");
            string allowedFileExtensions = GetSettingString("AllowedFileExtensions");
            string youtubeAPIKey = GetSettingString("YoutubeAPIKey");
            string thumbnailSizes = GetSettingString(nameof(Setting.ThumbnailSizes));


            return new Setting()
            {
                MainMenuId = mainMenuId,
                MainPageUrl = mainPageUrl,
                SiteName = siteName,
                Reviews = reviews,
                YaMetrika = yaMetrika,
                AnalyticsAuthKeyExpires = yaMetrikaAuthKeyExpires,
                Timezone = timezone,
                RootUrl = rootUrl,
                ImageMaxHeight = imageMaxHeight,
                ImageMaxWidth = imageMaxWidth,
                ImageQuality = imageQuality,
                AutoEmailReplyEnabled = autoEmailReplyEnabled,
                EmailSmtpUrl = emailSmtpUrl,
                EmailSmtpPort = emailSmtpPort,
                SmtpSslEnabled = smtpSslEnabled,
                EmailLogin = emailLogin,
                EmailPassword = emailPassword,
                OrderEmailAddress = orderEmailAddress,
                SystemEmailAddress = systemEmailAddress,
                SystemEmailSenderName = systemEmailSenderName,
                TranslitEnabled = translitEnabled,
                RootBreadcrumbsTitle = rootBreadcrumbsTitle,
                AllowedFileExtensions = allowedFileExtensions,
                YoutubeAPIKey = youtubeAPIKey,
                ThumbnailSizes = thumbnailSizes
            };


        }
        
        public int GetMainMenuId()
        {
            return GetSettingInt("MainMenuId");
        }

        public void SetAnalyticsAuthToken(string token, long expires)
        {
            UpdateOneSetting("AnalyticsAuthKey", token);
            DateTime expirationDate = DateTime.Now.AddSeconds(expires);
            UpdateOneSetting("AnalyticsAuthKeyExpires", expirationDate);
        }

        public DateTime GetAnalyticsAuthKeyExpirationDate()
        {
            return GetSettingDateTime("AnalyticsAuthKeyExpires");
        }

        public string GetAnalyticsAuthKey()
        {
            return GetSettingString("AnalyticsAuthKey");
        }

        public TimeSpan GetTicketLifetime()
        {
            try
            {
                string lifetime = GetSettingString("TicketLifetime");
                TimeSpan res = TimeSpan.Parse(lifetime);
                return res;
            }
            catch
            {
                return new TimeSpan(2, 0, 0);
            }
        }






        #region Private
        private void UpdateOneSetting(string key, DateTime value)
        {
            UpdateOneSetting(key, value.ToString("yyyy/MM/dd"));
        }
        private void UpdateOneSetting(string key, int value)
        {
            UpdateOneSetting(key, value.ToString(CultureInfo.InvariantCulture));
        }

        private void UpdateOneSetting(string key, bool value)
        {
            UpdateOneSetting(key, value.ToString(CultureInfo.InvariantCulture));
        }

        private void UpdateOneSetting<T>(string key, T value)
        {
            string setting = typeof(T) == typeof(string) ? value as string : Mapper.Map<string>(value);
            UpdateOneSetting(key, setting);
        }

        private void UpdateOneSetting(string key, string value)
        {
            if (value == null)
            {
                RemoveSetting(key);
            }
            else
            {
                _settingGateway.Upsert(key, value);
                AddOrUpdateCacheObject(key, value);
            }
        }

        private string GetSettingString(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            return GetFromCacheOrLoadAndAddToCache(key, () => _settingGateway.GetValue(key));
        }

        private int GetSettingInt(string key)
        {
            return Convert.ToInt32(GetSettingString(key));
        }

        private bool GetSettingBool(string key)
        {
            return Convert.ToBoolean(GetSettingString(key));
        }

        private decimal GetSettingDecimal(string key)
        {
            return Convert.ToDecimal(GetSettingString(key));
        }

        private DateTime GetSettingDateTime(string key)
        {
            return DateTime.Parse(GetSettingString(key));
        }


        #endregion

        protected override int CacheExpirationInMinutes => 30;
    }
}
