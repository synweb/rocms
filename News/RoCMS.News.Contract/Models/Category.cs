using System;
using System.Collections.Generic;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Search;

namespace RoCMS.News.Contract.Models
{
    public class Category: ISearchable
    {
        public Category()
        {
            ChildrenCategories = new List<Category>();
        }
        public int CategoryId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public int SortOrder { get; set; }
        public bool Hidden { get; set; }
        public ICollection<Category> ChildrenCategories { get; set; }
        public IdNamePair<int> ParentCategory { get; set; }

        public string RelativeUrl { get; set; }

        public string CanonicalUrl { get; set; }
        public string SearchKeyName => nameof(Name);
        public IEnumerable<string> SearchIndexKeys => new [] {SearchKeyName};
    }
}
