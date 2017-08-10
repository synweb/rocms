using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.News.Data.Models;

namespace RoCMS.News.Data.Gateways
{
    public class NewsItemTagGateway: NewsBaseGateway
    {
        public void Insert(int newsId, int tagId)
        {
            Exec(GetProcedureString(), new {newsItemId=newsId, tagId});
        }

        public void Delete(int newsId, int tagId)
        {
            Exec(GetProcedureString(), new { newsItemId = newsId, tagId });
        }

        public ICollection<TagStat> SelectTagStats(int tagCount)
        {
            return ExecSelect<TagStat>(GetProcedureString(), tagCount);
        }
    }
}
