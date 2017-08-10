using System;

namespace RoCMS.Shop.Contract.Models.Exceptions
{
    public class GoodsNotFoundException: Exception
    {
        public int GoodsId { get; private set; }
        public string RelativeUrl { get; private set; }

        public GoodsNotFoundException(int goodsId)
        {
            GoodsId = goodsId;
        }

        public GoodsNotFoundException(string relativeUrl)
        {
            RelativeUrl = relativeUrl;
        }
    }
}
