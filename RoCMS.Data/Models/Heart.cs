using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Data.Models
{
    public class Heart
    {
        public int HeartId { get; set; }
        public DateTime CreationDate { get; set; }
        public string RelativeUrl { get; set; }
        public int? ParentHeartId { get; set; }
        public string BreadcrumbsTitle { get; set; }
        public bool Noindex { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Styles { get; set; }
        public string Scripts { get; set; }
        public string Layout { get; set; }
        public string AdditionalHeaders { get; set; }
        public string Type { get; set; }
    }
}
