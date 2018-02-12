using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.News.Data.Models;

namespace RoCMS.News.Data.Gateways
{
    public class RssCrawlerFilterGateway: NewsBasicGateway<RssCrawlerFilter>
    {
        public ICollection<RssCrawlerFilter> SelectByRssCrawler(int rssCrawlerId)
        {
            return ExecSelect<RssCrawlerFilter>(GetProcedureString(), rssCrawlerId);
        }
    }
}
