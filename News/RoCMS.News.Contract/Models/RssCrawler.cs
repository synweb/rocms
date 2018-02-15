using System.Collections.Generic;
using RoCMS.Base.Models;

namespace RoCMS.News.Contract.Models
{
    public class RssCrawler
    {
        public RssCrawler()
        {
            Filters = new List<RssCrawlerFilter>();
        }

        public int RssCrawlerId { get; set; }
        public string RssFeedUrl { get; set; }
        public bool IsEnabled { get; set; }
        public int CheckInterval { get; set; }
        public int? TargetCategoryId { get; set; }
        public string ImageSelector { get; set; }
        public IdNamePair<int> TargetCategory { get; set; }

        public ICollection<RssCrawlerFilter> Filters { get; set; }
    }
}
