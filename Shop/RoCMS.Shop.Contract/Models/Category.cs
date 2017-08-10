using System.Collections.Generic;
using RoCMS.Base.Models;

namespace RoCMS.Shop.Contract.Models
{
    public class Category
    {
        public Category()
        {
            ChildrenCategories = new List<Category>();
            OrderFormSpecs = new List<Spec>();
        }


        public int CategoryId { get; set; }
        public System.Guid Guid { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }

        public ICollection<Category> ChildrenCategories { get; set; }


        public IdNamePair<int> ParentCategory { get; set; }

        public string MetaDescription { get; set; }
        public string ImageId { get; set; }

        public bool Hidden { get; set; }

        public string RelativeUrl { get; set; }

        public string CannonicalUrl { get; set; }
        public int SortOrder { get; set; }
        public ICollection<Spec> OrderFormSpecs { get; set; } 
    }
}
