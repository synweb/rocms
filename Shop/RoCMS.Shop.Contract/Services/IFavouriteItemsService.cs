using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IFavouriteItemsService
    {
        IList<FavouriteItem> GetFavouriteItems(Guid sessionId);
        void Add(Guid sessionId, int heartId);
        void Delete(Guid sessionId, int heartId);
        bool IsItemInFavourites(Guid sessionId, int heartId);
    }
}
