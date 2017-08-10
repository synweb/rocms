using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;

namespace RoCMS.Shop.Services
{
    class ShopPackService: BaseShopService, IShopPackService
    {
        private readonly DimensionGateway _dimensionGateway = new DimensionGateway();
        private readonly PackGateway _packGateway = new PackGateway();
        public IList<Dimension> GetDimensions()
        {
            var dataRes = _dimensionGateway.Select();
            var res = Mapper.Map<IList<Dimension>>(dataRes);
            return res;
        }

        public Pack GetPack(int packId)
        {
            // TODO: кэширование
            var dataRes = _packGateway.SelectOne(packId);
            var res = Mapper.Map<Pack>(dataRes);
            FillDimension(res);
            return res;
        }

        private void FillDimension(Pack pack)
        {
            var dataRec = _dimensionGateway.SelectOne(pack.DimensionId);
            var dim = Mapper.Map<Dimension>(dataRec);
            pack.Dimension = dim;
        }

        public int CreatePack(Pack pack)
        {
            var dataRec = Mapper.Map<Data.Models.Pack>(pack);
            int id = _packGateway.Insert(dataRec);
            return id;
        }

        public void UpdatePack(Pack pack)
        {
            var dataRec = Mapper.Map<Data.Models.Pack>(pack);
            _packGateway.Update(dataRec);
        }

        public void DeletePack(int packId)
        {
            _packGateway.Delete(packId);
        }

        public IList<Pack> GetPacks()
        {
            // TODO: кэширование
            var dataRes = _packGateway.Select();
            var res = Mapper.Map<IList<Pack>>(dataRes);
            foreach (var pack in res)
            {
                FillDimension(pack);
            }
            return res;
        }
    }
}
