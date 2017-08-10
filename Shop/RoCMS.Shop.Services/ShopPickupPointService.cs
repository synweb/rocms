using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Services
{
    class ShopPickupPointService: BaseShopService, IShopPickupPointService
    {
        private readonly PickUpPointGateway _pickUpPointGateway = new PickUpPointGateway();

        public PickupPointInfo GetPickupPoint(int id)
        {
            var dataRes = _pickUpPointGateway.SelectOne(id);
            var res = Mapper.Map<PickupPointInfo>(dataRes);
            return res;
        }
        public IList<PickupPointInfo> GetPickupPoints()
        {
            var dataRes = _pickUpPointGateway.Select();
            var res = Mapper.Map<IList<PickupPointInfo>>(dataRes);
            return res;
        }

        public void DeletePickupPoint(int id)
        {
            _pickUpPointGateway.Delete(id);
        }

        public void UpdatePickupPoint(PickupPointInfo point)
        {
            var dataRec = Mapper.Map<PickUpPoint>(point);
            _pickUpPointGateway.Update(dataRec);
        }

        public int CreatePickupPoint(PickupPointInfo point)
        {
            var dataRec = Mapper.Map<PickUpPoint>(point);
            return _pickUpPointGateway.Insert(dataRec);
        }
    }
}
