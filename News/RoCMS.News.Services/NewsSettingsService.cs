using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Services
{
    public class NewsSettingsService : INewsSettingsService
    {
        private readonly ISettingsService _settingsService;
        public NewsSettingsService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public NewsSettings GetNewsSettings()
        {
            string blogUrl = _settingsService.GetSettings<string>("BlogUrl");

            return new NewsSettings() { BlogUrl = blogUrl };
        }

        public void UpdateNewsSettings(NewsSettings settings)
        {
            _settingsService.Set("BlogUrl", settings.BlogUrl);
        }
    }
}
