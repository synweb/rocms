namespace RoCMS.Shop.Contract.Models
{
    public class GoodsAwaiting
    {
        public int GoodsAwaitingId { get; set; }
        public int HeartId { get; set; }
        public string Contact { get; set; }
        public ContactType ContactType { get; set; }
        public int? UserId { get; set; }
        public bool Sent { get; set; }
    }
}
