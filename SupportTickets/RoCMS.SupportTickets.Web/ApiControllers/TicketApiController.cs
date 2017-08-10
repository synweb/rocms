using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.SupportTickets.Contract;
using RoCMS.SupportTickets.Contract.Models;
using RoCMS.SupportTickets.Contract.Services;
using RoCMS.SupportTickets.Web.ViewModels;
using RoCMS.Web.Contract.Models.Security;
using RoCMS.Web.Contract.Services;

namespace RoCMS.SupportTickets.Web.ApiControllers
{
    [Authorize]
    public class TicketApiController: ApiController
    {
        private readonly ISupportTicketService _supportTicketService;
        private readonly IMapperService _mapper;
        private readonly IPrincipalResolver _resolver;

        public TicketApiController(ISupportTicketService supportTicketService, IMapperService mapper, IPrincipalResolver resolver)
        {
            _supportTicketService = supportTicketService;
            _mapper = mapper;
            _resolver = resolver;
        }

        [HttpGet]
        public ResultModel GetTicketVMsPage(int page, int pageSize)
        {
            int totalCount;
            var serviceRes = _supportTicketService.GetTickets(new PagingFilter {Page=page, PageSize = pageSize}, out totalCount);
            var res = _mapper.Map<ICollection<TicketVM>>(serviceRes);
            return new ResultModel(true, new {Tickets = res, TotalCount = totalCount});
        }

        [HttpGet]
        public Ticket GetTicket(int ticketId)
        {
            var ticket = _supportTicketService.GetTicket(ticketId);
            
            if (ticket.AuthorId != ((RoPrincipal)User).UserId && !((RoPrincipal)User).IsAuthorizedForResource(SupportTicketsRoCMSResources.SupportTickets))
            {
                throw new HttpException(403, "Доступ запрещен");
            }

            return ticket;
        }

        [HttpGet]
        public ResultModel GetTicketsPageForUser(int page, int pageSize)
        {
            int totalCount;
            int userId = _resolver.GetUserId();
            var serviceRes = _supportTicketService.GetTicketsForUser(userId, new PagingFilter { Page = page, PageSize = pageSize }, out totalCount);
            var res = _mapper.Map<ICollection<TicketVM>>(serviceRes);
            return new ResultModel(true, new { Tickets = res, TotalCount = totalCount });
        }

        [HttpPost]
        public ResultModel CreateMessage(CreateMessageData data)
        {

            var userId = ((RoPrincipal) User).UserId;
            var msg = new Message
            {
                AuthorId = userId,
                Text = data.Text,
                TicketId = data.TicketId

            };
            int id = _supportTicketService.CreateMessage(msg);
            return new ResultModel(true, id);
        }

        [HttpGet]
        public Message GetMessage(int id)
        {
            var userId = ((RoPrincipal) User).UserId;
            Message m = _supportTicketService.GetMessage(id);
            if (m.AuthorId != userId && !((RoPrincipal)User).IsAuthorizedForResource(SupportTicketsRoCMSResources.SupportTickets))
            {
                throw new HttpException(403, "Доступ запрещен");
            }
            return m;
        }

        public class CreateMessageData
        {
            public int TicketId { get; set; }
            public string Text { get; set; }
        }

        public class CreateTicketData
        {
            public TicketType TicketType { get; set; }
            public string Subject { get; set; }
            public string Text { get; set; }
        }

        [HttpPost]
        public ResultModel ResolveTicket(int id)
        {
            _supportTicketService.ResolveTicket(id);
            return ResultModel.Success;
        }

        [HttpPost]
        public ResultModel CreateTicket(CreateTicketData data)
        {
            var userId = ((RoPrincipal) User).UserId;
            var msg = new Message
            {
                AuthorId = userId,
                Text = data.Text,
            };
            var ticket = new Ticket()
            {
                AuthorId = userId,
                Subject = data.Subject,
                TicketType = data.TicketType
            };
            int id = _supportTicketService.CreateTicket(ticket, msg);
            return new ResultModel(true, id);
        }
    }
}