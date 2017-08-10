using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class PageGateway:BasicGateway<Page>
    {
        public Page SelectByRelativeUrl(string url)
        {
            return Exec<Page>(GetProcedureString(), url);
        }

        public ICollection<Page> SelectChildren(int parentPageId)
        {
            return ExecSelect<Page>(GetProcedureString(), parentPageId);
        }
    }
}
