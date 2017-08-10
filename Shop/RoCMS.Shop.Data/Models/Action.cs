using System;

namespace RoCMS.Shop.Data.Models
{
    public class Action
    {
        public int ActionId { get; set; }
        public Nullable<System.DateTime> DateOfEnding { get; set; }
        public string Description { get; set; }
        public int Discount { get; set; }
        public string Name { get; set; }
        public string ImageId { get; set; }
        public bool ShowInSlider { get; set; }
        public bool Active { get; set; }
    }
}
