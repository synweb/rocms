using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class HeartGateway: BasicGateway<Heart>
    {
        public Heart SelectByRelativeUrl(string url)
        {
            return Exec<Heart>(GetProcedureString(), url);
        }

        public ICollection<Heart> SelectChildren(int parentHeartId)
        {
            return ExecSelect<Heart>(GetProcedureString(), parentHeartId);
        }

        public ICollection<Heart> SelectByType(string type)
        {
            return ExecSelect<Heart>(GetProcedureString(), type);
        }
    }
}
