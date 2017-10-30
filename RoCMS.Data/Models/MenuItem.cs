using System;

namespace RoCMS.Data.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }

        public int MenuId { get; set; }
        public Nullable<int> ParentMenuItemId { get; set; }
        public int? HeartId { get; set; }
        public int SortOrder { get; set; }
        public Nullable<int> BlockId { get; set; }
    }
}
