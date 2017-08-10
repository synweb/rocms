using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopCompatiblesService
    {
        CompatibleSet GetCompatibleSet(int compatibleSetId);
        int CreateCompatibleSet(CompatibleSet compatibleSet);
        void UpdateCompatibleSet(CompatibleSet compatibleSet);
        void DeleteCompatibleSet(int compatibleSetId);
        IList<CompatibleSet> GetCompatibleSets();
    }
}
