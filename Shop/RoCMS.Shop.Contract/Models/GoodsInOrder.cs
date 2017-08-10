namespace RoCMS.Shop.Contract.Models
{
    public class GoodsInOrder
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int GoodsId { get; set; }
        public int OrderId { get; set; }

        public int? PackId { get; set; }
        public Pack Pack { get; set; }



        public decimal BasePrice
        {
            get
            {
                if (PackId.HasValue)
                {
                    return Goods.PriceForPack(PackId.Value);
                }
                else
                {
                    return Goods.Price;
                }
            }
        }

        public decimal Price { get; set; }

        public GoodsItem Goods { get; set; }
    }
}
