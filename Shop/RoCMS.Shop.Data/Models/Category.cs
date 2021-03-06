﻿using System;

namespace RoCMS.Shop.Data.Models
{
    public class Category
    {
        public int HeartId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentCategoryId { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

        public string ImageId { get; set; }
        public bool Hidden { get; set; }
        public System.Guid Guid { get; set; }

    }
}
