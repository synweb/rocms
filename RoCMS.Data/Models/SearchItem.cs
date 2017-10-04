using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Data.Models
{
    public class SearchItem
    {
        public string SearchItemKey { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string SearchContent { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Weight { get; set; }
        public string ImageId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? HeartId { get; set; }
    }
}
