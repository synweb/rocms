using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class InterfaceStringGateway: BaseGateway
    {
        public ICollection<InterfaceString> Select()
        {
            return ExecSelect<InterfaceString>(GetProcedureString());
        }

        public InterfaceString SelectOne(string key)
        {
            return Exec<InterfaceString>(GetProcedureString(), key);
        }

        public void Upsert(InterfaceString rec)
        {
            Exec(GetProcedureString(), rec);
        }

        public void Delete(string key)
        {
            Exec(GetProcedureString(), key);
        }
    }
}
