using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoCMS.Base.Models;
using RoCMS.SupportTickets.Contract;
using RoCMS.SupportTickets.Contract.Models;
using RoCMS.SupportTickets.Contract.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;
using Message = RoCMS.SupportTickets.Contract.Models.Message;

namespace RoCMS.SupportTickets.Services
{
    class SupportTicketService: BaseService, ISupportTicketService
    {
        private IMailService _mailService;
        private IPrincipalResolver _principal;

        public SupportTicketService(IMapperService mapper, IMailService mailService, IPrincipalResolver principal)
            : base(mapper)
        {
            _mailService = mailService;
            _principal = principal;
        }

        public ICollection<Ticket> GetTickets(PagingFilter paging, out int totalCount)
        {
            using (var db = Context)
            {
                var dataRes = db.Tickets.Where(x => !x.Deleted)
                    .OrderBy(x => x.Resolved)
                    .ThenByDescending(x => x.CreationDate)
                    .Skip((paging.Page - 1)*paging.PageSize)
                    .Take(paging.PageSize);
                var res = _mapper.Map<ICollection<Ticket>>(dataRes);
                totalCount = db.Tickets.Count(x => !x.Deleted);
                return res;
            }
        }

        public ICollection<Ticket> GetTicketsForUser(int userId, PagingFilter paging, out int totalCount)
        {
            using (var db = Context)
            {
                var dataRes = db.Tickets.Where(x => !x.Deleted && x.AuthorId == userId)
                    .OrderBy(x => x.Resolved)
                    .ThenByDescending(x => x.CreationDate)
                    .Skip((paging.Page - 1) * paging.PageSize)
                    .Take(paging.PageSize);
                var res = _mapper.Map<ICollection<Ticket>>(dataRes);
                totalCount = db.Tickets.Count(x => !x.Deleted);
                return res;
            }
        }

        public int CreateTicket(Ticket ticket, Message message)
        {
            ticket.CreationDate = DateTime.UtcNow;
            message.CreationDate = DateTime.UtcNow;
            var dataTicket = _mapper.Map<Data.Ticket>(ticket);
            var dataMessage = _mapper.Map<Data.Message>(message);
            using (var db = Context)
            {
                
                db.Tickets.Add(dataTicket);
                db.SaveChanges();
                dataMessage.TicketId = dataTicket.TicketId;
                db.Messages.Add(dataMessage);
                db.SaveChanges();
            }

            Task t = new Task(() =>
            {
                _mailService.Send(new MailMsg()
                {
                    Body = "Поступило новое обращение в техподдержку от клиента №" + ticket.AuthorId,
                    Receiver = SupportTicketsAppSettingsHelper.SupportEmailAddress,
                    Subject = "Новое обращение в техподдержку от клиента №" + ticket.AuthorId
                });
            });
            t.Start();
            return dataTicket.TicketId;
        }

        public int CreateMessage(Message message)
        {
            var dataMessage = _mapper.Map<Data.Message>(message);
            dataMessage.CreationDate = DateTime.UtcNow;
            using (var db = Context)
            {
                db.Messages.Add(dataMessage);
                db.SaveChanges();
            }

            var user = _principal.GetCurrentUser();
            
            if (!user.IsAuthorizedForResource(SupportTicketsRoCMSResources.SupportTickets))//то есть, не админ
            {
                Task t = new Task(() =>
                {
                    _mailService.Send(new MailMsg()
                    {
                        Body = "Поступило новое сообщение в техподдержку от клиента №" + message.AuthorId,
                        Receiver = SupportTicketsAppSettingsHelper.SupportEmailAddress,
                        Subject = "Новое сообщение в техподдержку от клиента №" + message.AuthorId
                    });
                });
                t.Start();
            }
            return dataMessage.MessageId;
        }

        public void ResolveTicket(int ticketId)
        {
            using (var db = Context)
            {
                db.Tickets.Find(ticketId).Resolved = true;
                db.SaveChanges();
            }
        }

        public Ticket GetTicket(int ticketId)
        {
            using (var db = Context)
            {
                var dataRes = db.Tickets.Find(ticketId);
                var res = _mapper.Map<Ticket>(dataRes);
                res.Messages = res.Messages.OrderByDescending(x => x.CreationDate).ToList();

                return res;
            }
        }

        public void DeleteTicket(int ticketId)
        {
            using (var db = Context)
            {
                var ticket = db.Tickets.Find(ticketId);
                if (ticket.Deleted)
                {
                    throw new KeyNotFoundException();
                }
                ticket.Deleted = true;
                db.SaveChanges();
            }
        }

        public void DeleteMessage(int messageId)
        {
            using (var db = Context)
            {
                var message = db.Messages.Find(messageId);
                if (message.Deleted)
                {
                    throw new KeyNotFoundException();
                }
                message.Deleted = true;
                db.SaveChanges();
            }
        }

        public void ReadMessages(int ticketId)
        {
            using (var db = Context)
            {
                var ticket = db.Tickets.Find(ticketId);
                foreach (var message in ticket.Messages)
                {
                    message.IsRead = true;
                }
                db.SaveChanges();
            }
        }

        public Message GetMessage(int id)
        {
            using (var db = Context)
            {
                var msg = db.Messages.Find(id);
                if (msg != null)
                {
                    return _mapper.Map<Message>(msg);
                }
                throw new ArgumentException();
            }
        }

    }
}
