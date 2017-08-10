using System;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class PasswordTicketGateway: BaseGateway
    {
        public int Insert(PasswordTicket rec)
        {
            return Exec<int>(GetProcedureString(), rec);
        }

        public PasswordTicket SelectAvailableByToken(Guid token)
        {
            return Exec<PasswordTicket>(GetProcedureString(), token);
        }

        public void UseTicket(int ticketId)
        {
            Exec(GetProcedureString(), ticketId);
        }
    }
}
