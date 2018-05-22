namespace RoCMS.Shop.Contract.Models
{
    public class Pack
    {
        public int PackId { get; set; }
        public System.Guid Guid { get; set; }
        public string Name { get; set; }

        public string FullName { get; set; }

        public double Size { get; set; }
        
        public int DimensionId { get; set; }
        public Dimension Dimension { get; set; }
        public int? DefaultDiscount { get; set; }
    }
}
