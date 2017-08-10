using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.SupportTickets.Contract
{
    public static class SupportTicketsAppSettingsHelper
    {
        public static string SupportEmailAddress
        {
            get { return ConfigurationManager.AppSettings.Get("SupportEmailAddress"); }
        }
    }
}
