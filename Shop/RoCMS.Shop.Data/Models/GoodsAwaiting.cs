using System;

namespace RoCMS.Shop.Data.Models
{
    public class GoodsAwaiting
    {
        public int GoodsAwaitingId { get; set; }
        public int HeartId { get; set; }
        public string Contact { get; set; }
        public string ContactType { get; set; }
        public Nullable<int> UserId { get; set; }
        public bool Sent { get; set; }
    }
}
