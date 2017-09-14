using System;

namespace RoCMS.Web.Contract.Models
{
    public class Setting
    {
        public int MainMenuId { get; set; }
        public string SiteName { get; set; }
        public string MainPageUrl { get; set; }
        public bool Reviews { get; set; }
        public int YaMetrika { get; set; }
        public DateTime? AnalyticsAuthKeyExpires { get; set; }
        public int Timezone { get; set; }
        public string RootUrl { get; set; }
        public int ImageMaxHeight { get; set; }
        public int ImageMaxWidth { get; set; }
        public int ImageQuality { get; set; }
        public bool AutoEmailReplyEnabled { get; set; }
        public string EmailSmtpUrl { get; set; }
        public int EmailSmtpPort { get; set; }
        public bool SmtpSslEnabled { get; set; }
        public string EmailLogin { get; set; }
        public string EmailPassword { get; set; }
        public string OrderEmailAddress { get; set; }
        public string SystemEmailAddress { get; set; }
        public string SystemEmailSenderName { get; set; }
        public bool TranslitEnabled { get; set; }
        public string RootBreadcrumbsTitle { get; set; }
        public string AllowedFileExtensions { get; set; }
        public string YoutubeAPIKey { get; set; }
        public string ThumbnailSizes { get; set; }
        public bool ReviewCreatedNotification { get; set; }
        public ReviewSort ReviewSort { get; set; }
    }
}
