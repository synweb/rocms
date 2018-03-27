using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        private const string BLOCK_CACHE_KEY_TEMPLATE = "Block:Id:{0}";
        private const string BLOCK_CACHE_BY_NAME_KEY_TEMPLATE = "Block:Name:{0}";
        private readonly BlockGateway _blockGateway = new BlockGateway();
       
        protected override int CacheExpirationInMinutes => AppSettingsHelper.HoursToExpireCartCache * 60;

        public BlockService()
        {
            InitCache("BlockServiceMemoryCache");
        }

        #region IBlockService 
        public int CreateBlock(Block block)
        {
            GuardCheck(block);
            return _blockGateway.Insert(Mapper.Map<Data.Models.Block>(block));
        }

        private void GuardCheck(Block block)
        {
            if (!string.IsNullOrEmpty(block.Name))
            {
                const string NAME_REGEX =
                    @"^(([a-zA-Zа-яА-ЯёЁ]{1})|([a-zA-Zа-яА-ЯёЁ]{1}[a-zA-Zа-яА-ЯёЁ]{1})|([a-zA-Zа-яА-ЯёЁ]{1}[0-9]{1})|([0-9]{1}[a-zA-Zа-яА-ЯёЁ]{1})|([a-zA-Zа-яА-ЯёЁ0-9][a-zA-Zа-яА-ЯёЁ0-9-_]{1,195}[a-zA-Zа-яА-ЯёЁ0-9]))$";
                if (!Regex.IsMatch(block.Name, NAME_REGEX))
                {
                    throw new ArgumentException(nameof(block.Name));
                }
            }
        }

        public void DeleteBlock(int blockId)
        {
            var block = _blockGateway.SelectOne(blockId);
            _blockGateway.Delete(blockId);
            RemoveObjectFromCache(GetBlockCacheKey(blockId));
            RemoveObjectFromCache(GetBlockCacheKey(block.Name));
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

        public Block GetBlock(string name)
        {
            string cacheKey = GetBlockCacheKey(name);
            return GetFromCacheOrLoadAndAddToCache(cacheKey, () =>
            {
                var newBlock = _blockGateway.SelectByName(name);
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
            GuardCheck(block);
            _blockGateway.Update(Mapper.Map<Data.Models.Block>(block));
            RemoveObjectFromCache(GetBlockCacheKey(block.BlockId));
            RemoveObjectFromCache(GetBlockCacheKey(block.Name));
        }
        #endregion

        private string GetBlockCacheKey(int blockId)
        {
            return String.Format(BLOCK_CACHE_KEY_TEMPLATE, blockId);
        }

        private string GetBlockCacheKey(string blockName)
        {
            return String.Format(BLOCK_CACHE_BY_NAME_KEY_TEMPLATE, blockName);
        }
    }
}
