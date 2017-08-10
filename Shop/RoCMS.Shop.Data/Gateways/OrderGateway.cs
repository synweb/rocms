using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class OrderGateway: ShopBasicGateway<Order>
    {
        public ICollection<Order> SelectPage(int startIndex, int count, out int total, int? clientId)
        {
            var param = new SelectPageParam()
            {
                StartIndex = startIndex,
                Count = count,
                TotalCount = 0,
                ClientId = clientId
            };
            var res = ExecSelect<Order>(GetProcedureString(), param);
            total = param.TotalCount;
            return res;
        }

        private class SelectPageParam
        {
            public int StartIndex { get; set; }
            public int Count { get; set; }
            public int TotalCount { get; set; }
            public int? ClientId { get; set; }
        }
    }
}
