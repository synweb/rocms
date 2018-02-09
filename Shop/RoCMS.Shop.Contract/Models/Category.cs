using System.Collections.Generic;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Shop.Contract.Models
{
    public class Category: Heart
    {
        public Category()
        {
            ChildrenCategories = new List<Category>();
            OrderFormSpecs = new List<Spec>();
        }


        public System.Guid Guid { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }

        public ICollection<Category> ChildrenCategories { get; set; }


        public IdNamePair<int> ParentCategory { get; set; }

        
        public string ImageId { get; set; }

        public bool Hidden { get; set; }

        public int SortOrder { get; set; }
        public ICollection<Spec> OrderFormSpecs { get; set; } 
    }
}
