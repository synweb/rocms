using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class BlockGateway: BasicGateway<Block>
    {
        public Block SelectByName(string name)
        {
            return Exec<Block>(GetProcedureString(), name);
        }
    }
}
