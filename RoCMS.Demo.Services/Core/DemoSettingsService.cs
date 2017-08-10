using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AutoMapper;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Demo.Services.Core
{
    public class DemoSettingsService: ISettingsService
    {
        private Setting _defaultSetting;

        public DemoSettingsService()
        {
            try
            {
                var file = "blocks.xml";
                var xs = new XmlSerializer(typeof(Setting));
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoData", file);
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    _defaultSetting = (Setting) xs.Deserialize(fs);
                }
            }
            catch
            {
                _defaultSetting = new Setting()
                {
                    AllowedFileExtensions = "",
                    RootUrl = "http://demo.rocms.ru",
                    SystemEmailSenderName = "RoCMS",
                    EmailLogin = "mail",
                    EmailPassword = "",
                    EmailSmtpPort = 465,
                    SmtpSslEnabled = true,
                    EmailSmtpUrl = "smtp.rocms.ru",
                    AutoEmailReplyEnabled = false,
                    OrderEmailAddress = "mail@rocms.ru",
                    SystemEmailAddress = "mail@rocms.ru",
                    ImageQuality = 90,
                    ImageMaxWidth = 1920,
                    ImageMaxHeight = 1080,
                    MainMenuId = 1,
                    Reviews = false,
                    SiteName = "RoCMS Demo Website",
                    MainPageUrl = "home",
                    ThumbnailSizes = "300w,200h",
                    RootBreadcrumbsTitle = "Home",
                    Timezone = 3,
                    TranslitEnabled = true
                };
            }
        }

        public void UpdateSettings(Setting model)
        {
            // настройки менять не даём
        }

        public void UpdateEmailPassword(string password)
        {
            // настройки менять не даём
        }

        public string GetSiteName()
        {
            return _defaultSetting.SiteName;
        }

        public string GetHomepageUrl()
        {
            return _defaultSetting.MainPageUrl;
        }

        public Setting GetSettings()
        {
            return _defaultSetting;
        }

        public int GetMainMenuId()
        {
            return _defaultSetting.MainMenuId;
        }

        public void SetAnalyticsAuthToken(string token, long expires)
        {
            // настройки менять не даём
        }

        public DateTime GetAnalyticsAuthKeyExpirationDate()
        {
            return new DateTime(3017, 1, 22);
        }

        public string GetAnalyticsAuthKey()
        {
            return string.Empty;
        }

        public TimeSpan GetTicketLifetime()
        {
            return new TimeSpan(0, 0, 2);
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

        private string GetSettingString(string key)
        {
            var prop = _defaultSetting.GetType().GetProperty(key);
            return prop.GetValue(_defaultSetting).ToString();
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

        private void UpdateOneSetting(string key, string setting)
        {
            // настройки менять не даём
        }

        public string[] GetEmailTemplateNames()
        {
            return new string[0];
        }
    }
}
