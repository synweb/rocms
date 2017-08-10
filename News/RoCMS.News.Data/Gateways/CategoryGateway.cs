using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.News.Data.Models;

namespace RoCMS.News.Data.Gateways
{
    public class CategoryGateway: NewsBaseGateway
    {
        protected override string DefaultScheme => "News";

        public ICollection<Category> Select(int? parentId=null)
        {
            return ExecSelect<Category>(GetProcedureString(), parentId);
        }

        public ICollection<Category> SelectAll()
        {
            return ExecSelect<Category>(GetProcedureString());
        }

        public Category SelectOne(int id)
        {
            return Exec<Category>(GetProcedureString(), id);
        }

        public Category SelectByUrl(string relativeUrl)
        {
            return Exec<Category>(GetProcedureString(), relativeUrl);
        }

        public int Insert(Category record)
        {
            return Exec<int>(GetProcedureString(), record);
        }

        public void Delete(int id)
        {
            Exec(GetProcedureString(), id);
        }

        public void Update(Category record)
        {
            Exec(GetProcedureString(), record);
        }
    }
}
