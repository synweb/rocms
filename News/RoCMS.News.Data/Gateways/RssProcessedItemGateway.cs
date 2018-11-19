using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.News.Data.Gateways
{
    public class RssProcessedItemGateway : NewsBasicGateway<Models.RssProcessedItem>
    {
        public Models.RssProcessedItem SelectByRssSource(string rssSource)
        {
            return Exec<Models.RssProcessedItem>(GetProcedureString(), rssSource);
        }
    }
}
