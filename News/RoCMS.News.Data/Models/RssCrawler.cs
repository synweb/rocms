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
        public int? TargetCategoryId { get; set; }
        public string ImageSelector { get; set; }
        public string ContentContainerSelector { get; set; }
        public string LinkText { get; set; }
        public string Tags { get; set; }
        public string ExcludeTags { get; set; }
    }
}
