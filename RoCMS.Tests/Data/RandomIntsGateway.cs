using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;

namespace RoCMS.Tests.Data
{
    public class RandomIntsGateway:BaseGateway
    {
        public ICollection<RandomInts> Select()
        {
            return ExecSelect<RandomInts>(GetProcedureString(nameof(Select)));
        }

        public ICollection<RandomInts> SelectManyBySelectOne(int count)
        {
            var res = new List<RandomInts>();
            for(int i = 1; i<=count; i++)
            {
                res.Add(Exec<RandomInts>(GetProcedureString("SelectOne")));
            }
            return res;
        }
    }
}
