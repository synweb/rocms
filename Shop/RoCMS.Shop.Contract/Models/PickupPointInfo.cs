namespace RoCMS.Shop.Contract.Models
{
    public class PickupPointInfo
    {
        public int PickUpPointId { get; set; }

        public string Title { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Metro { get; set; }
        public string Schedule { get; set; }
        public string Description { get; set; }
        public string PaymentType { get; set; }
        public string ImageId { get; set; }
        public string Partner { get; set; }
        public string Phone { get; set; }
        public string HowToReach { get; set; }
    }
}
