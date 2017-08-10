using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopAwaitingService
    {
        void SendNotifications(int goodsId);

        void SendNotifications(int[] goodsId);

        int CreateGoodsAwaiting(GoodsAwaiting goodsAwaiting);

        void DeleteGoodsAwaiting(int goodsAwaitingId);
    }
}
