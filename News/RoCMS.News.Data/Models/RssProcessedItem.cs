using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Data.Models
{
    public class RssProcessedItem
    {
        public int RssProcessedItemId { get; set; }
        public string RssSource { get; set; }
        public DateTime CreationDate { get; set; }
        public int? NewsItemId { get; set; }
    }
}
