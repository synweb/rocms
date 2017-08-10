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
        private readonly SpecGateway _specGateway = new SpecGateway();
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
            return id;
        }

        public void UpdateSpec(Spec spec)
        {
            var dataRec = Mapper.Map<Data.Models.Spec>(spec);
            _specGateway.Update(dataRec);
        }

        public void DeleteSpec(int specId)
        {
            _specGateway.Delete(specId);
        }

        public IList<Spec> GetSpecs()
        {
            var dataRes = _specGateway.Select().OrderBy(x => x.SortOrder);
            var res = Mapper.Map<IList<Spec>>(dataRes);
            return res;
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
        }
    }
}
