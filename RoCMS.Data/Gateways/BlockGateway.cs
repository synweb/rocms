using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class BlockGateway: BaseGateway
    {
        public int Insert(Block block)
        {
            return Exec<int>("[dbo].[Block_Insert]", block);
        }

        public void Delete(int id)
        {
            Exec("[dbo].[Block_Delete]", id);
        }

        public Block SelectOne(int id)
        {
            return Exec<Block>("[dbo].[Block_SelectOne]", id);
        }

        public ICollection<Block> Select()
        {
            return ExecSelect<Block>("[dbo].[Block_Select]");

        }

        public void Update(Block block)
        {
            Exec("[dbo].[Block_Update]", block);
        }
    }
}
