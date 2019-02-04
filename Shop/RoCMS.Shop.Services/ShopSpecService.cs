using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;

namespace RoCMS.Shop.Services
{
    public class ShopSpecService: BaseShopService, IShopSpecService
    {
        const string CACHE_KEY_ALL_SPECS = "ALLSPECS";
        private readonly SpecGateway _specGateway = new SpecGateway();


        public ShopSpecService()
        {
            InitCache("ShopSpecService");
        }


        public Spec GetSpec(int specId)
        {
            var dataRes = _specGateway.SelectOne(specId);
            var res = Mapper.Map<Spec>(dataRes);
            return res;
        }

        public int CreateSpec(Spec spec)
        {
            var dataRec = Mapper.Map<Data.Models.Spec>(spec);
            int id = _specGateway.Insert(dataRec);

            RemoveObjectFromCache(CACHE_KEY_ALL_SPECS);

            return id;
        }

        public void UpdateSpec(Spec spec)
        {
            var dataRec = Mapper.Map<Data.Models.Spec>(spec);
            _specGateway.Update(dataRec);
            RemoveObjectFromCache(CACHE_KEY_ALL_SPECS);
        }

        public void DeleteSpec(int specId)
        {
            _specGateway.Delete(specId);
            RemoveObjectFromCache(CACHE_KEY_ALL_SPECS);
        }

        public IList<Spec> GetSpecs()
        {
            return GetFromCacheOrLoadAndAddToCache(CACHE_KEY_ALL_SPECS, () =>
            {
                var dataRes = _specGateway.Select().OrderBy(x => x.SortOrder);
                var res = Mapper.Map<IList<Spec>>(dataRes);
                return res;
            });
        }

        public void UpdateSpecOrder(ICollection<int> specIds)
        {
            using (var ts = new TransactionScope())
            {
                for (int i = 0; i < specIds.Count; i++)
                {
                    var spec = _specGateway.SelectOne(specIds.ElementAt(i));
                    spec.SortOrder = i;
                    _specGateway.Update(spec);
                }
                ts.Complete();
            }

            RemoveObjectFromCache(CACHE_KEY_ALL_SPECS);
        }
    }
}
