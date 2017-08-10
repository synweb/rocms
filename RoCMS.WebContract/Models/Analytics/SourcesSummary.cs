namespace RoCMS.Web.Contract.Models.Analytics
{
    public class SourcesSummary
    {
        public int Direct { get; set; }

        public int Internal { get; set; }

        public int Social { get; set; }

        public int Links { get; set; }

        public int SavedPages { get; set; }

        public int Ads { get; set; }

        public int Mail { get; set; }

        public int Undefined { get; set; }

        public int Search { get; set; }
    }
}
