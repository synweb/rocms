using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoCMS.Web.Contract.Models.Shop;

namespace RoCMS.ViewModels
{
    public class OrderedGoodsVM
    {
        public GoodsItem Goods { get; set; }

        public int Quantity { get; set; }

        public decimal Sum { get; set; }
    }
}