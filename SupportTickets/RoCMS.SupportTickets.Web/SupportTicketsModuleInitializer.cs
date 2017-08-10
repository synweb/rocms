using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using RoCMS.Base.Infrastructure;
using RoCMS.SupportTickets.Contract.Models;
using RoCMS.SupportTickets.Web.ViewModels;

namespace RoCMS.SupportTickets.Web
{
    public class SupportTicketsModuleInitializer : IModuleInitializer
    {
        private const string MODULE_DIR = "supportTickets";

        public void Init()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles, MODULE_DIR);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            CreateMappings();
        }

        private void CreateMappings()
        {
            Mapper.CreateMap<Ticket, TicketVM>()
                .ForMember(d => d.HasUnreadMessages, x => x.MapFrom(y => y.Messages.Any(m => !m.IsRead)));
        }
    }
}