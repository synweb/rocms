using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class CountryGateway: BaseGateway
    {
        public ICollection<Country> Select()
        {
            return ExecSelect<Country>(GetProcedureString());
        }
        public Country SelectOne(int id)
        {
            return Exec<Country>(GetProcedureString(), id);
        }
    }
}
