using System;

namespace RoCMS.Shop.Data.Models
{
    public class Cart
    {
        public System.Guid CartId { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<decimal> TotalDiscount { get; set; }
    }
}
