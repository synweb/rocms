using System;

namespace RoCMS.Shop.Data.Models
{
    public class Pack
    {
        public int PackId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public int DimensionId { get; set; }
        public Nullable<int> DefaultDiscount { get; set; }
        public double Size { get; set; }
        public System.Guid Guid { get; set; }
    }
}
