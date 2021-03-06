﻿using System;
using System.Collections.Generic;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Web.ViewModels
{
    public class OrderVM
    {

        public int OrderId { get; set; }
        public DateTime CreationDate { get; set; }
        public OrderState State { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public IEnumerable<OrderedGoodsVM> Goods { get; set; }
        public Client Client { get; set; }
        public decimal Total { get; set; }

    }
}