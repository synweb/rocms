using RoCMS.Base.Data;
using RoCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Data.Gateways
{
    public class MailGateway : BaseGateway
    {
        public int Insert(Mail mail)
        {
            return Exec<int>("[dbo].[Mail_Insert]", mail);
        }

        public void Delete(int mailId)
        {
            Exec("[dbo].[Mail_Delete]", mailId);
        }

        public ICollection<Mail> Select()
        {
            return ExecSelect<Mail>("[dbo].[Mail_Select]");
        }

        public Mail SelectOne(int mailId)
        {
            return Exec<Mail>("[dbo].[Mail_SelectOne]", mailId);
        }

        public void Update(Mail mail)
        {
            Exec("[dbo].[Mail_Update]", mail);
        }
    }
}
