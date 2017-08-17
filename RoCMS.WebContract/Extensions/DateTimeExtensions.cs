using System;
using System.Web.Mvc;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Contract.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ApplySiteTimezone(this DateTime dateTime)
        {
            var settingsService = DependencyResolver.Current.GetService<ISettingsService>();
            int timezone = settingsService.GetSettings<int>(nameof(Setting.Timezone));
            return dateTime.AddHours(timezone);
        }
    }
}
