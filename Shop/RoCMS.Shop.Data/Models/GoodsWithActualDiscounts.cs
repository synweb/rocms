using System;

namespace RoCMS.Shop.Data.Models
{
    public class GoodsWithActualDiscounts
    {
        public int HeartId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ManufacturerId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string HtmlDescription { get; set; }
        public string MainImageId { get; set; }
        public string Article { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public bool NotAvailable { get; set; }
        public Nullable<decimal> ActualPrice { get; set; }
        public string Currency { get; set; }
    }
}
