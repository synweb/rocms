using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class SettingGateway:BaseGateway
    {
        public string GetValue(string key)
        {
            return Exec<string>(GetProcedureString(), key);
        }

        public void Delete(string key)
        {
            Exec(GetProcedureString(), key, true);
        }

        public ICollection<Setting> Select()
        {
            return ExecSelect<Setting>(GetProcedureString());
        }

        public void Upsert(string key, string value)
        {
            Exec(GetProcedureString(), new { key, value }, true);
        }
    }
}
