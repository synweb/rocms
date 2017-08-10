namespace RoCMS.Shop.Data.Models
{
    public class GoodsPack
    {
        public int PackId { get; set; }
        public int GoodsId { get; set; }
        public int? Discount { get; set; }
        public decimal? Price { get; set; }
    }
}
