namespace RoCMS.Shop.Contract.Models
{
    public class ShopSettings
    {
        public decimal DeliveryCost { get; set; }
        public decimal SelfPickupCost { get; set; }

        public string CourierCities { get; set; }

        public string ShopUrl { get; set; }
    }
}
