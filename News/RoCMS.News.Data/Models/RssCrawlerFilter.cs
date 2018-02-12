using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Data.Models
{
    public class RssCrawlerFilter
    {
        public int RssCrawlerFilterId { get; set; }
        public int RssCrawlerId { get; set; }
        public string Filter { get; set; }
    }
}
