using System;
using System.Linq;

namespace RoCMS.Shop.Contract.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.Guid CartId { get; set; }
        public int GoodsId { get; set; }
        public GoodsItem GoodsItem { get; set; }

        public int? PackId { get; set; }

        public GoodsPack Pack
        {
            get
            {
                return PackId.HasValue ? GoodsItem.Packs.First(x => x.PackInfo.PackId == PackId) : null;
            }
        }

        public decimal DiscountedPrice
        {
            get
            {
                return Math.Round(PackId.HasValue
                    ? GoodsItem.MainCurrDiscountedPriceForPack(PackId.Value)
                    : GoodsItem.MainCurrDiscountedPrice, 0, MidpointRounding.AwayFromZero);
            }
        }

        public int Quantity { get; set; }

        public decimal Summary
        {
            get { return Math.Round(DiscountedPrice * Quantity, 0, MidpointRounding.AwayFromZero); }
        }
    }
}
