using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;

namespace RoCMS.News.Data.Gateways
{
    public class NewsItemCategoryGateway:NewsBaseGateway
    {
        protected override string TableName => "NewsItem_Category";

        public void Insert(int newsItemId, int categoryId)
        {
            Exec(GetProcedureString(),new {newsItemId, categoryId});
        }

        public void Delete(int newsItemId, int categoryId)
        {
            Exec(GetProcedureString(), new { newsItemId, categoryId });
        }

        public ICollection<int> SelectNewsIdsByCategory(int categoryId)
        {
            return ExecSelect<int>(GetProcedureString("SelectByCategory"), categoryId);
        }

        public ICollection<int> SelectCategoryIdsByNews(int newsId)
        {
            return ExecSelect<int>(GetProcedureString("SelectByNews"), newsId);
        }
    }
}
