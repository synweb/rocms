using RoCMS.Base.Data;
using RoCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Data.Gateways
{
    public class FormRequestGateway : BaseGateway
    {
        public int Insert(FormRequest formRequest)
        {
            return Exec<int>("[dbo].[FormRequest_Insert]", formRequest);
        }

        public void Delete(int formRequestId)
        {
            Exec("[dbo].[FormRequest_Delete]", formRequestId);
        }

        public ICollection<FormRequest> Select()
        {
            return ExecSelect<FormRequest>("[dbo].[FormRequest_Select]");
        }

        public FormRequest SelectOne(int formRequestId)
        {
            return Exec<FormRequest>("[dbo].[FormRequest_Select]", formRequestId);
        }

        public void Update(FormRequest formRequest)
        {
             Exec("[dbo].[FormRequest_Update]", formRequest);
        }
    }
}
