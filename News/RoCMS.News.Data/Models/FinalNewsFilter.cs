using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Models;

namespace RoCMS.News.Data.Models
{
    public class FinalNewsFilter
    {
        public ICollection<int> NewsIds { get; set; }
        public bool? OnlyFutureEventDate { get; set; }
        public int? BlogId { get; set; }
        public ICollection<string> RecordTypes { get; set; }
        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
        public bool OnlyPosted { get; set; }
    }
}
