namespace RoCMS.Shop.Export.Contract.Models
{
    public class YmlExportSettings
    {
        public string SiteName { get; set; }
        public string SiteDescription { get; set; }
        public string SiteUrl { get; set; }
        public decimal ClickRate { get; set; }
        public decimal DeliveryCost { get; set; }
        public bool Pickup { get; set; }

    }
}
