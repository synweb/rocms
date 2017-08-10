using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.SupportTickets.Contract.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public string Subject { get; set; }
        public int AuthorId { get; set; }
        public TicketType TicketType { get; set; }
        public bool Resolved { get; set; }
        public bool Deleted { get; set; }
        public System.DateTime CreationDate { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
