using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Web.ViewModels
{
    public class OrderedGoodsVM
    {
        public GoodsItem Goods { get; set; }

        public int Quantity { get; set; }

        public decimal Sum { get; set; }
    }
}