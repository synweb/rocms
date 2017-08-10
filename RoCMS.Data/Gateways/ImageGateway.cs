using System;
using System.Data;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class ImageGateway: BaseGateway
    {
        public void Insert(Image rec)
        {
            Exec(GetProcedureString(), rec);
        }

        public void Delete(string id)
        {
            Exec(GetProcedureString(), id, true);
        }

        public void Update(Image rec)
        {
            Exec(GetProcedureString(), rec);
        }

        public Image SelectOne(string imageId)
        {
            return Exec<Image>(GetProcedureString(), imageId);
        }
    }
}
