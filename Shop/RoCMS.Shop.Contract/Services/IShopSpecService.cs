using System.Collections.Generic;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopSpecService
    {
        Spec GetSpec(int specId);
        int CreateSpec(Spec spec);
        void UpdateSpec(Spec spec);
        void DeleteSpec(int specId);
        IList<Spec> GetSpecs();
        void UpdateSpecOrder(ICollection<int> specIds);
    }
}
