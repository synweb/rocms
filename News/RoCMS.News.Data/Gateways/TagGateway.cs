using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.News.Data.Models;

namespace RoCMS.News.Data.Gateways
{
    public class TagGateway: NewsBaseGateway
    {
        public int Insert(string name)
        {
            return Exec<int>(GetProcedureString(), name);
        }

        public void Delete(int id)
        {
            Exec(GetProcedureString(), id);
        }

        public Tag SelectOne(int id)
        {
            return Exec<Tag>(GetProcedureString(), id);
        }

        public ICollection<Tag> Select()
        {
            return ExecSelect<Tag>(GetProcedureString());
        }

        public ICollection<Tag> SelectByNews(int newsId)
        {
            return ExecSelect<Tag>(GetProcedureString(), newsId);
        }

        public void DeleteUnassociated()
        {
            Exec(GetProcedureString(), null, true);
        }

        public ICollection<string> SelectByPattern(string pattern, int records)
        {
            return ExecSelect<string>(GetProcedureString(), new {pattern, records});
        }
    }
}
