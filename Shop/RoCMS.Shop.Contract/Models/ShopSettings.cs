﻿using System.Collections.Generic;
using RoCMS.Base.Models;

namespace RoCMS.Shop.Contract.Models
{
    public class ShopSettings
    {
        public decimal DeliveryCost { get; set; }
        public decimal SelfPickupCost { get; set; }

        public string CourierCities { get; set; }

        public string ShopUrl { get; set; }

        public int DefaultPageSize { get; set; }

        public List<IdNamePair<int>> SpecsInFilter { get; set; }
    }
}
