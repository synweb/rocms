using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public int SortOrder { get; set; }
        public bool Hidden { get; set; }

        public string RelativeUrl { get; set; }
    }
}
