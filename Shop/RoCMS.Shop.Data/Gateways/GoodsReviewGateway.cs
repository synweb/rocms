﻿using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class GoodsReviewGateway: ShopBasicGateway<GoodsReview>
    {
        public ICollection<GoodsReview> SelectByGoods(int heartId)
        {
            return ExecSelect<GoodsReview>(GetProcedureString(), heartId);
        }
    }
}
