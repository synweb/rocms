using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;

namespace RoCMS.News.Data.Gateways
{
    public abstract class NewsBaseGateway:BaseGateway
    {
        protected override string DefaultScheme => "News";
    }
}
