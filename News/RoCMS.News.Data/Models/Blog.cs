using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Data.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string RelativeUrl { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? OwnerId { get; set; }
    }
}
