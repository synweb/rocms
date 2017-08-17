using System;
using System.Collections.Generic;
using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class BlockService : BaseCoreService, IBlockService
    {
        private const string BLOCK_CACHE_KEY_TEMPLATE = "Block:{0}";
        private readonly BlockGateway _blockGateway = new BlockGateway();
       
        protected override int CacheExpirationInMinutes => AppSettingsHelper.HoursToExpireCartCache * 60;

        #region IBlockService 
        public int CreateBlock(Block block)
        {
            return _blockGateway.Insert(Mapper.Map<Data.Models.Block>(block));
        }

        public void DeleteBlock(int blockId)
        {
            _blockGateway.Delete(blockId);
            RemoveObjectFromCache(GetBlockCacheKey(blockId));
        }

        public Block GetBlock(int id)
        {
            string cacheKey = GetBlockCacheKey(id);
            return GetFromCacheOrLoadAndAddToCache(cacheKey, () =>
            {
                var newBlock = _blockGateway.SelectOne(id);
                return Mapper.Map<Block>(newBlock);
            });
        }

        public IEnumerable<IdNamePair<int>> GetBlockIdNames()
        {
            var blocks = _blockGateway.Select();
            var list = Mapper.Map<ICollection<IdNamePair<int>>>(blocks);
            return list;
        }

        public IList<Block> GetBlocks()
        {
            var blocks = _blockGateway.Select();
            var list = Mapper.Map<ICollection<Block>>(blocks);
            return new List<Block>(list);
        }

        public void UpdateBlock(Block block)
        {
            _blockGateway.Update(Mapper.Map<Data.Models.Block>(block));
            RemoveObjectFromCache(GetBlockCacheKey(block.BlockId));
        }
        #endregion

        private string GetBlockCacheKey(int blockId)
        {
            return String.Format(BLOCK_CACHE_KEY_TEMPLATE, blockId);
        }
    }
}
