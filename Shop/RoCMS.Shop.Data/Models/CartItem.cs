using System;

namespace RoCMS.Shop.Data.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.Guid CartId { get; set; }
        public int GoodsId { get; set; }
        public Nullable<int> PackId { get; set; }
        public int Quantity { get; set; }

    }
}
