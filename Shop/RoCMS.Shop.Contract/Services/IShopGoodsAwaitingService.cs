using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopGoodsAwaitingService
    {
        void SendNotifications(int heartId);

        void SendNotifications(int[] goodsId);

        int CreateGoodsAwaiting(GoodsAwaiting goodsAwaiting);

        void DeleteGoodsAwaiting(int goodsAwaitingId);
    }
}
