using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Data.Models
{
    public class SearchResultItem
    {
        public string SearchItemKey { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Weight { get; set; }
        public string ImageId { get; set; }
        public int Rank { get; set; }
        public int Relevance => Weight*Rank;
    }
}
