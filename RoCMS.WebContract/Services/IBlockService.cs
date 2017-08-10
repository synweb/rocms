using System.Collections.Generic;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IBlockService
    {
        int CreateBlock(Block block);

        Block GetBlock(int id);

        IList<Block> GetBlocks();

        void UpdateBlock(Block block);

        void DeleteBlock(int blockId);
        IEnumerable<IdNamePair<int>> GetBlockIdNames();
    }
}
