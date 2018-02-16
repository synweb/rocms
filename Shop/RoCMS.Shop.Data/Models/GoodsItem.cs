using System;

namespace RoCMS.Shop.Data.Models
{
    public class GoodsItem
    {
        public int HeartId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ManufacturerId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public System.DateTime DateOfAddition { get; set; }

        public string Description { get; set; }
        public string HtmlDescription { get; set; }
        public string MainImageId { get; set; }
        public string Article { get; set; }
        public string SearchDescription { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public bool NotAvailable { get; set; }
        public Nullable<int> BasePackId { get; set; }
        public bool Deleted { get; set; }
        public System.Guid Guid { get; set; }

        public string Currency { get; set; }
        public string Filename { get; set; }
    }
}
