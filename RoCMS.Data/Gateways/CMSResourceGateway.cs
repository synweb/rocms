using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class CMSResourceGateway: BasicGateway<CMSResource>
    {
        protected override string TableName => "CmsResource";

        public CMSResource SelectByName(string name)
        {
            return Exec<CMSResource>(GetProcedureString(), name);
        }

    }
}
