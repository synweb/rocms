using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class FavouriteItemGateway : ShopBaseGateway
    {
        public ICollection<FavouriteItem> Select(Guid sessionId)
        {
            return ExecSelect<FavouriteItem>(GetProcedureString(), sessionId);
        }

        public void Insert(FavouriteItem item)
        {
            Exec(GetProcedureString(), item);
        }

        public void Delete(FavouriteItem item)
        {
            Exec(GetProcedureString(), item);
        }
    }
}
