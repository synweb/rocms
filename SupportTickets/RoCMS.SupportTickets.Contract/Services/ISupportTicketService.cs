using System.Collections.Generic;
using RoCMS.Base.Models;
using RoCMS.SupportTickets.Contract.Models;

namespace RoCMS.SupportTickets.Contract.Services
{
    public interface ISupportTicketService
    {
        ICollection<Ticket> GetTickets(PagingFilter paging, out int totalCount);
        ICollection<Ticket> GetTicketsForUser(int userId, PagingFilter paging, out int totalCount);
        int CreateTicket(Ticket ticket, Message message);
        int CreateMessage(Message message);
        void ResolveTicket(int ticketId);
        Ticket GetTicket(int ticketId);
        void DeleteTicket(int ticketId);
        void DeleteMessage(int messageId);
        void ReadMessages(int ticketId);
        Message GetMessage(int id);
    }
}
