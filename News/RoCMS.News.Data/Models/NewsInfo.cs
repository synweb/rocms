using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Data.Models
{
    public class NewsInfo
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public System.DateTime PostingDate { get; set; }
        public string Description { get; set; }
        public string MetaDescription { get; set; }
        public string Keywords { get; set; }
        public System.DateTime CreationDate { get; set; }
        public int AuthorId { get; set; }
        public string ImageId { get; set; }
        public string RelativeUrl { get; set; }
        public Nullable<int> CommentTopicId { get; set; }
        public RecordType RecordType { get; set; }
        public string Filename { get; set; }
        public string VideoId { get; set; }

        public int BlogId { get; set; }

        public int? RelatedNewsItemId { get; set; }

        public DateTime? EventDate { get; set; }
        public string AdditionalHeaders { get; set; }
    }
}
