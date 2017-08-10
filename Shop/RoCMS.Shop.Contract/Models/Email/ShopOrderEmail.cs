namespace RoCMS.Shop.Contract.Models.Email
{
    public class ShopOrderEmail
    {
        public Order Order { get; set; }
        public Client Client { get; set; }
    }
}
