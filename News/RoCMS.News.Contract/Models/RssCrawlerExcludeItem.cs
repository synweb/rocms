using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Contract.Models
{
    public class RssCrawlerExcludeItem
    {
        public int ExcludeItemIndex { get; set; }
        public int RssCrawlerId { get; set; }
        public string Selector { get; set; }
    }
}
