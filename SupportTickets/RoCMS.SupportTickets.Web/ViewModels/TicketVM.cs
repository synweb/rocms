using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoCMS.SupportTickets.Web.ViewModels
{
    public class TicketVM
    {
        public int TicketId { get; set; }
        public string Subject { get; set; }
        public int AuthorId { get; set; }
        public string TicketType { get; set; }
        public bool Resolved { get; set; }
        public DateTime CreationDate { get; set; }
        public bool HasUnreadMessages { get; set; }
    }
}