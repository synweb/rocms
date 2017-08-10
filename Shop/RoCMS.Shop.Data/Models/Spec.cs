namespace RoCMS.Shop.Data.Models
{
    public class Spec
    {
        public int SpecId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ValueType { get; set; }
        public string AcceptableValues { get; set; }
        public System.Guid Guid { get; set; }
        public string Prefix { get; set; }
        public string Postfix { get; set; }
        public int SortOrder { get; set; }
    }
}
