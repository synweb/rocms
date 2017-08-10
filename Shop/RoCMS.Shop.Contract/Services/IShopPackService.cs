using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopPackService
    {
        IList<Dimension> GetDimensions();
        Pack GetPack(int packId);
        int CreatePack(Pack pack);
        void UpdatePack(Pack pack);
        void DeletePack(int packId);
        IList<Pack> GetPacks();
    }
}
