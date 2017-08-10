using System;

namespace RoCMS.Data.Models
{
    public class PasswordTicket
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public System.Guid Token { get; set; }
        public bool Used { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public Nullable<System.DateTime> UseDate { get; set; }
    }
}
