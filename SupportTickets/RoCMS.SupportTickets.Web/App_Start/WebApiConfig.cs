using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using JetBrains.Annotations;
using RoCMS.Helpers;

namespace RoCMS.SupportTickets.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ApiRoute(config.Routes, "tickets/{page}/{pageSize}/get", "TicketApi", "GetTicketVMsPage");
            ApiRoute(config.Routes, "tickets/{page}/{pageSize}/user/get", "TicketApi", "GetTicketsPageForUser");
            ApiRoute(config.Routes, "ticket/{ticketId}/get", "TicketApi", "GetTicket");
            ApiRoute(config.Routes, "tickets/{ticketId}/get", "TicketApi", "GetTicket");
            ApiRoute(config.Routes, "ticket/create", "TicketApi", "CreateTicket");
            ApiRoute(config.Routes, "ticket/{id}/resolve", "TicketApi", "ResolveTicket");
            ApiRoute(config.Routes, "message/send", "TicketApi", "CreateMessage");

            ApiRoute(config.Routes, "message/{id}/get", "TicketApi", "GetMessage");

        }


        private static void ApiRoute(HttpRouteCollection routes, string url, [AspMvcController] string controller,
            [AspMvcAction] string action)
        {
            url = String.Format("{0}/{1}/{2}", ConstantStrings.WebApiExecutionPath, "support", url);
            HttpRouteValueDictionary defaults = new HttpRouteValueDictionary();
            if (controller != null)
            {
                defaults["controller"] = controller;
            }
            if (action != null)
            {
                defaults["action"] = action;
            }
            IHttpRoute route = routes.CreateRoute(url, defaults, new Dictionary<string, object>());
            routes.Add(route.RouteTemplate, route);
        }
    }
}
