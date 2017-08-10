using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class UserCMSResourceGateway: BaseGateway
    {
        protected override string TableName => "UserCmsResource";

        public bool CheckIfAuthorizedForResource(int userId, string resourceName)
        {
            return Exec<bool>(GetProcedureString(), new {userId, resourceName});
        }

        public void Insert(int userId, int cmsResourceId)
        {
            Exec(GetProcedureString(), new {userId, cmsResourceId});
        }

        public ICollection<CMSResource> SelectByUser(int userId)
        {
            return ExecSelect<CMSResource>(GetProcedureString(), userId);
        }

        public void Delete(int userId, int cmsResourceId)
        {
            Exec(GetProcedureString(), new { userId, cmsResourceId });
        }
    }
}
