using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RoCMS.Base.Helpers;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Models.Security;
using RoCMS.Web.Services.Base;

namespace RoCMS.Shop.Services
{
    public class ShopAwaitingService : ShopContextService, IShopAwaitingService
    {


        public ShopAwaitingService()
        {
            
        }

        protected override int CacheExpirationInMinutes
        {
            get { return AppSettingsHelper.HoursToExpireCartCache * 60; }
        }


        public void SendNotifications(int goodsId)
        {
            throw new NotImplementedException();
        }

        public void SendNotifications(int[] goodsId)
        {
            throw new NotImplementedException();
        }

        public int CreateGoodsAwaiting(GoodsAwaiting goodsAwaiting)
        {
            using (var db = Context)
            {
                Data.GoodsAwaiting dataAwaiting = _mapper.Map<Data.GoodsAwaiting>(goodsAwaiting);
                db.GoodsAwaiting.Add(dataAwaiting);
                db.SaveChanges();
                return dataAwaiting.GoodsAwaitingId;
            }
        }

        public void DeleteGoodsAwaiting(int goodsAwaitingId)
        {
            using (var db = Context)
            {
                var gaw = db.GoodsAwaiting.Find(goodsAwaitingId);
                if(gaw == null) return;
                db.GoodsAwaiting.Remove(gaw);
                db.SaveChanges();
            }
        }
    }
}
