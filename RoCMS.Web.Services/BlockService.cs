using RoCMS.Web.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models;
using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.Base.Services;
using RoCMS.Data.Gateways;
using Block = RoCMS.Data.Models.Block;

namespace RoCMS.Web.Services
{
    public class BlockService : BaseCoreService, IBlockService
    {
        private const string BLOCK_CACHE_KEY_TEMPLATE = "Block:{0}";
        private readonly BlockGateway _blockGateway = new BlockGateway();
       
        protected override int CacheExpirationInMinutes
        {
            get
            {
                return AppSettingsHelper.HoursToExpireCartCache * 60;
            }
        }

        #region реализация IBlockService 
        public int CreateBlock(RoCMS.Web.Contract.Models.Block block)
        {
            return _blockGateway.Insert(Mapper.Map<Block>(block));
        }

        public void DeleteBlock(int blockId)
        {
            _blockGateway.Delete(blockId);
        }

        public RoCMS.Web.Contract.Models.Block GetBlock(int id)
        {
            var newBlock = _blockGateway.SelectOne(id);
            return Mapper.Map<RoCMS.Web.Contract.Models.Block>(newBlock);
        }

        public IEnumerable<IdNamePair<int>> GetBlockIdNames()
        {
            var blocks = _blockGateway.Select();
            var list = Mapper.Map<ICollection<IdNamePair<int>>>(blocks);
            return list;
        }

        public IList<Contract.Models.Block> GetBlocks()
        {
            var blocks = _blockGateway.Select();
            var list = Mapper.Map<ICollection<RoCMS.Web.Contract.Models.Block>>(blocks);
            return new List<RoCMS.Web.Contract.Models.Block>(list);
        }

        public void UpdateBlock(Contract.Models.Block block)
        {
            _blockGateway.Update(Mapper.Map<Block>(block));
        }
        #endregion

        private string GetBlockCacheKey(int blockId)
        {
            return String.Format(BLOCK_CACHE_KEY_TEMPLATE, blockId);
        }
    }
}
