using System;

namespace RoCMS.Shop.Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Address { get; set; }
        public int ClientId { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Comment { get; set; }
        public OrderState State { get; set; }
        public Nullable<System.DateTime> ShipmentDate { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public Nullable<int> PickUpPointId { get; set; }
        public string AdminComment { get; set; }
        public Nullable<decimal> DeliveryPrice { get; set; }
        public int TotalDiscount { get; set; }

        public PaymentType PaymentType { get; set; }
        public PaymentState? PaymentState { get; set; }
    }
}
