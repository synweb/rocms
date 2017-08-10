using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Services
{
    public interface IPasswordTicketService
    {
        Guid CreateTicket(string email);
        void UseTicket(Guid token, string password);
    }
}
