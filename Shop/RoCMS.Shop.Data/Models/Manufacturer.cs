using System;

namespace RoCMS.Shop.Data.Models
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public string LogoImageId { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public Nullable<int> CountryId { get; set; }
        public System.Guid Guid { get; set; }
    }
}
