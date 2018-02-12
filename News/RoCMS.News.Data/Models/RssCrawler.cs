using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Data.Models
{
    public class RssCrawler
    {
        public int RssCrawlerId { get; set; }
        public string RssFeedUrl { get; set; }
        public bool IsEnabled { get; set; }
        public int CheckInterval { get; set; }
        public int TargetCategory { get; set; }
    }
}
