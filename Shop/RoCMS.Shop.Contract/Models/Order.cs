using System;
using System.Collections.Generic;
using System.Linq;

namespace RoCMS.Shop.Contract.Models
{
    public class Order
    {
        public Order()
        {
            GoodsInOrder = new List<GoodsInOrder>();
        }

        public int OrderId { get; set; }
        public System.DateTime CreationDate { get; set; }

        public string PostCode { get; set; }
        public string City { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Metro { get; set; }

        public string FrontNumber { get; set; }

        public string HouseIndex { get; set; }

        public string Appartment { get; set; }

        public string Floor { get; set; }

        public string Intercom { get; set; }

        public int? PickUpPointId { get; set; }


        public int ClientId { get; set; }
        public string Comment { get; set; }
        public List<GoodsInOrder> GoodsInOrder { get; set; }
        public OrderState State { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentState? PaymentState { get; set; }
        /// <summary>
        /// Заказавший клиент. Использовать только для отображения!
        /// </summary>
        public Client Client { get; set; }

        public PickupPointInfo PickUpPoint { get; set; }

        public string AdminComment { get; set; }

        public decimal Summary
        {
            get
            {
                decimal sum = GoodsInOrder.Sum(x => x.Price*(decimal) x.Quantity);
                sum = sum - sum*(decimal) TotalDiscount/100m;
                if (DeliveryPrice.HasValue)
                {
                    sum += DeliveryPrice.Value;
                }
                return Math.Round(sum, 2); 
            }
        }

        public int TotalDiscount { get; set; }

        public decimal? DeliveryPrice { get; set; }
    }
}
