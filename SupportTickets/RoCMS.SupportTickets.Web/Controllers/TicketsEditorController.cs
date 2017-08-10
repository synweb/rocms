using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.SupportTickets.Contract;
using RoCMS.SupportTickets.Contract.Services;

namespace RoCMS.SupportTickets.Web.Controllers
{
    [AuthorizeResources(SupportTicketsRoCMSResources.SupportTickets)]
    public class TicketsEditorController: Controller
    {
        private readonly ISupportTicketService _supportTicketService;

        public TicketsEditorController(ISupportTicketService supportTicketService)
        {
            _supportTicketService = supportTicketService;
        }

        [MvcSiteMapNode(Title = "Поддержка", ParentKey = "AdminHome", Key = "SupportTickets", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""SupportTickets"", ""visibility"": ""AdminMenu"", ""iconClass"" : ""fa-question"" }")]
        public ActionResult Index()
        {
            //int totalCount;
            //var tickets = _supportTicketService.GetTickets(new PagingFilter(), out totalCount);
            return View();
        }

        public ActionResult Ticket(int id)
        {
            //int totalCount;
            //var tickets = _supportTicketService.GetTickets(new PagingFilter(), out totalCount);
            var ticket = _supportTicketService.GetTicket(id);
            _supportTicketService.ReadMessages(id);
            return View(ticket);
        }
    }
}