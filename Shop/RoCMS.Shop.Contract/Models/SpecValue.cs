namespace RoCMS.Shop.Contract.Models
{
    public class SpecValue
    {
        public Spec Spec { get; set; }
        public int GoodsId { get; set; }
        public int SpecId { get; set; }
        public string Value { get; set; }
        public bool IsPrimary { get; set; }
    }
}
