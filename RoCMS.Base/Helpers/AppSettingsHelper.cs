using System.Configuration;

namespace RoCMS.Base.Helpers
{
    public static class AppSettingsHelper
    {

        public static int TempFilesCount
        {
            get { return int.Parse(ConfigurationManager.AppSettings.Get("TempFilesCount")); }
        }

        public static int MinutesToExpireTempFileCache
        {
            get { return int.Parse(ConfigurationManager.AppSettings.Get("MinutesToExpireTempFileCache")); }
        }

        public static int MaxTempFileSizeMb
        {
            get { return int.Parse(ConfigurationManager.AppSettings.Get("MaxTempFileSizeMb")); }
        }

        public static int HoursToExpireCartCache
        {
            get { return int.Parse(ConfigurationManager.AppSettings.Get("HoursToExpireCartCache")); }
        }

        public static string EmailBlindCarbonCopyAddress
        {
            get { return ConfigurationManager.AppSettings.Get("EmailBlindCarbonCopyAddress"); }
        }

        public static bool EmailBlindCarbonCopyEnabled
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings.Get("EmailBlindCarbonCopyEnabled"));
            }
        }

        public static bool RoCMSDemoMode
        {
            get
            {
                var stringVal = ConfigurationManager.AppSettings.Get("RoCMSDemoMode");
                return stringVal != null && bool.Parse(stringVal);
            }
        }
    }
}
