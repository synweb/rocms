namespace RoCMS.Shop.Data.Models
{
    public class Currency
    {
        public string CurrencyId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public decimal Rate { get; set; }
        public int SortOrder { get; set; }
        public bool IsMain { get; set; }
    }
}
