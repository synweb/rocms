using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models.Search;

namespace RoCMS.News.Contract.Models
{
    public class Blog: ISearchable
    {
        public int BlogId { get; set; }
        public string RelativeUrl { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? OwnerId { get; set; }
        public string SearchKeyRelativeUrl => nameof(RelativeUrl);
        public string SearchKeyTitle => nameof(Title);
        public string SearchKeySubtitle => nameof(Subtitle);
        public IEnumerable<string> SearchIndexKeys => new[] {SearchKeyRelativeUrl, SearchKeySubtitle, SearchKeyTitle};
    }
}
