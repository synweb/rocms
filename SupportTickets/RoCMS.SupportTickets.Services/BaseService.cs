using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.SupportTickets.Data;
using RoCMS.Web.Contract.Services;
using Message = RoCMS.SupportTickets.Contract.Models.Message;
using Ticket = RoCMS.SupportTickets.Contract.Models.Ticket;

namespace RoCMS.SupportTickets.Services
{
    public abstract class BaseService
    {
        protected readonly IMapperService _mapper;

        protected BaseService(IMapperService mapper)
        {
            _mapper = mapper;

            Mapper.CreateMap<Data.Message, Message>();
            Mapper.CreateMap<Message, Data.Message>().ForMember(x => x.Ticket, x => x.Ignore());

            Mapper.CreateMap<Data.Ticket, Ticket>();
            Mapper.CreateMap<Ticket, Data.Ticket>();
            Mapper.AssertConfigurationIsValid();
        }

        protected TicketsContainer Context
        {
            get
            {
                return new TicketsContainer();
            }
        }
    }
}
