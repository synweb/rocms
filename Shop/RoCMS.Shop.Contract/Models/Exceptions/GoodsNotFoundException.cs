using System;

namespace RoCMS.Shop.Contract.Models.Exceptions
{
    public class GoodsNotFoundException: Exception
    {
        public int HeartId { get; private set; }
        public string RelativeUrl { get; private set; }

        public GoodsNotFoundException(int heartId)
        {
            HeartId = heartId;
        }

        public GoodsNotFoundException(string relativeUrl)
        {
            RelativeUrl = relativeUrl;
        }
    }
}
