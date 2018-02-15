using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Data.Models;

namespace RoCMS.News.Data.Models
{
    public class NewsItem
    {
        public int HeartId { get; set; }
        public string Text { get; set; }
        public System.DateTime PostingDate { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public int? AuthorId { get; set; }
        public string ImageId { get; set; }
        public Nullable<int> CommentTopicId { get; set; }
        public RecordType RecordType { get; set; }
        public string Filename { get; set; }
        public string VideoId { get; set; }
        public string RssSource { get; set; }
        public int? BlogId { get; set; }

        public DateTime? EventDate { get; set; }
    }
}
