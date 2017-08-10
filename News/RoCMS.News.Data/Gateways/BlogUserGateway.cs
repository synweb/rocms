using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.News.Data.Models;

namespace RoCMS.News.Data.Gateways
{
    public class BlogUserGateway: NewsBaseGateway
    {
        protected override string TableName => "Blog_User";

        public void Insert(int blogId, int userId)
        {
            Exec(GetProcedureString(), new { blogId, userId });
        }

        public void Delete(int blogId, int userId)
        {
            Exec(GetProcedureString(), new { blogId, userId });
        }

        public ICollection<BlogUser> SelectByUser(int userId)
        {
            return ExecSelect<BlogUser>(GetProcedureString(), userId);
        }

        public ICollection<BlogUser> SelectByBlog(int blogId)
        {
            return ExecSelect<BlogUser>(GetProcedureString(), blogId);
        }
    }
}
