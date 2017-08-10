using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.News.Data.Models;

namespace RoCMS.News.Data.Gateways
{
    public class BlogGateway: BasicGateway<Blog>
    {
        protected override string DefaultScheme => "News";

        public Blog SelectOneByRelativeUrl(string relativeUrl)
        {
            return Exec<Blog>(GetProcedureString(), relativeUrl);
        }

        public ICollection<Blog> SelectByOwner(int ownerId)
        {
            return ExecSelect<Blog>(GetProcedureString(), ownerId);
        }
    }
}
