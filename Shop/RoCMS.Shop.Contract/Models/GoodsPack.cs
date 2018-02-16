namespace RoCMS.Shop.Contract.Models
{
    public class GoodsPack
    {
        public Pack PackInfo { get; set; }

        public int? Discount { get; set; }

        public decimal? Price { get; set; }
        public int PackId { get; set; }
        public int HeartId { get; set; }
    }
}
