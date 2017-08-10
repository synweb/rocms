using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.SupportTickets.Contract.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int AuthorId { get; set; }
        public int TicketId { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public bool Deleted { get; set; }
        public System.DateTime CreationDate { get; set; }

        //public Ticket Ticket { get; set; }
    }
}
