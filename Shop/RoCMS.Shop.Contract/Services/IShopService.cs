using System.Collections.Generic;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopService
    {
        GoodsItem GetGoods(int goodsId, bool activeActionsOnly = true);
        GoodsItem GetGoods(string relativeUrl, bool activeActionsOnly = true);
        int CreateGoods(GoodsItem goods);
        void UpdateGoods(GoodsItem goods);
        void DeleteGoods(int goodsId);
        IList<GoodsItem> GetGoodsSet(GoodsFilter filter, int startIndex, int count, out int totalCount, out FilterCollections collections, bool activeActionsOnly = true);
        int[] GetGoodsIds(string searchPattern);
        IList<Country> GetCountries();
        IList<GoodsItem> GetBestSellers(int count);
        IList<GoodsItem> GetNewGoodsItems(int count);
        bool GoodsExists(int id);

        bool GoodsExists(string relativeUrl);

        IList<GoodsItem> GetRecommendedGoods(int count, int[] categoryids, int currentGoodsId);
        
    }
}
