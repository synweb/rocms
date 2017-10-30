using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Search;

namespace RoCMS.News.Contract.Models
{
    [DisplayName(@"Новость")]
    public class NewsItem: Heart, ISearchable
    {
        public NewsItem()
        {
            this.Categories = new List<IdNamePair<int>>();
        }

        public ICollection<IdNamePair<int>> Categories { get; set; }

        [AllowHtml]
        public string Text { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_Reqiured")]
        public DateTime PostingDate { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_Reqiured")]
        public string Description { get; set; }

        public int AuthorId { get; set; }

        public string ImageId { get; set; }

        public int? CommentTopicId { get; set; }
        public string Tags { get; set; }
        public RecordType RecordType { get; set; }
        public string Filename { get; set; }
        public string VideoId { get; set; }

        public IEnumerable<string> SearchIndexKeys => new[]
        { SearchKeyTitle, SearchKeyDescription, SearchKeyText };

        public string SearchKeyTitle => nameof(Title);
        public string SearchKeyDescription => nameof(Description);
        public string SearchKeyText => nameof(Text);

        public int? BlogId { get; set; }

        public DateTime? EventDate { get; set; }
    }
}
