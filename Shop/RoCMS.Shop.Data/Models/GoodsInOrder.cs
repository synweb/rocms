using System;

namespace RoCMS.Shop.Data.Models
{
    public class GoodsInOrder
    {
        public int Quantity { get; set; }
        public int GoodsId { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> PackId { get; set; }
        public int Id { get; set; }
        public decimal Price { get; set; }
    }
}
