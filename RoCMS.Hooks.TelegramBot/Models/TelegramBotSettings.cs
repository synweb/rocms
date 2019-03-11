using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Hooks.TelegramBot.Models
{
    public class TelegramBotSettings
    {
        public string ProxyServer { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }
        public string ApiKey { get; set; }
        public string AllowedUserPhones { get; set; }
        public string WebHookToken { get; set; }
    }
}
