using System.Collections.Generic;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopActionService
    {

        Action GetAction(int actionId);
        int CreateAction(Action action);
        void UpdateAction(Action action);
        void DeleteAction(int actionId);
        IList<Action> GetActions();
        bool ActionExists(int id);
        IList<ActionShortInfo> GetActiveActionsForGoodsItem(int goodsItemId);
        IList<ActionShortInfo> GetActionsForGoodsItem(int goodsItemId);
        IList<Action> GetActionsForSlider();
        IList<Action> GetActiveActions();
    }
}
