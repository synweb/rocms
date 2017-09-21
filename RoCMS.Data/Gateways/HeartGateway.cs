using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class HeartGateway: BasicGateway<Heart>
    {
        public void FillHeart(Heart heart)
        {
            if (heart.HeartId == 0)
            {
                throw new ArgumentException(nameof(Heart.HeartId));
            }
            Heart dbHeart = SelectOne(heart.HeartId);
            heart.Fill(dbHeart);
        }
        public Heart SelectByRelativeUrl(string url)
        {
            return Exec<Heart>(GetProcedureString(), url);
        }

        public ICollection<Heart> SelectChildren(int parentHeartId)
        {
            return ExecSelect<Heart>(GetProcedureString(), parentHeartId);
        }
    }
}
