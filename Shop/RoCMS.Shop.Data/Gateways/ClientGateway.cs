using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class ClientGateway: ShopBasicGateway<Client>
    {
        public Client SelectOneByUserId(int userId)
        {
            return Exec<Client>(GetProcedureString(), userId);
        }

        public void UpdateInfo(Client client)
        {
            Exec(GetProcedureString(), client);
        }

        public ICollection<Client> SelectPage(int startIndex, int count, out int total)
        {
            var param = new SelectPageParam()
            {
                StartIndex = startIndex,
                Count = count,
                TotalCount = 0
            };
            var res = ExecSelect<Client>(GetProcedureString(), param);
            total = param.TotalCount;
            return res;
        }

        private class SelectPageParam
        {
            public int StartIndex { get; set; }
            public int Count { get; set; }
            public int TotalCount { get; set; }
        }
    }
}
