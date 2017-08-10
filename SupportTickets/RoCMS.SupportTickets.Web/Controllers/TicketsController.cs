using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Base.Models;
using RoCMS.SupportTickets.Contract.Services;

namespace RoCMS.SupportTickets.Web.Controllers
{
    [Authorize]
    public class TicketsController: Controller
    {
        private readonly ISupportTicketService _supportTicketService;

        public TicketsController(ISupportTicketService supportTicketService)
        {
            _supportTicketService = supportTicketService;
        }

        public ActionResult Tickets()
        {
            //int totalCount;
            //var tickets = _supportTicketService.GetTickets(new PagingFilter(), out totalCount);
            return View();
        }
    }
}