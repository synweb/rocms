using System;

namespace RoCMS.Data.Models
{
    public class Page
    {
        public string Title { get; set; }
        public string Annotation { get; set; }
        public string Content { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string RelativeUrl { get; set; }
        public string Keywords { get; set; }
        public int PageId { get; set; }
        public Nullable<int> ParentPageId { get; set; }
        public bool HideInSitemap { get; set; }
        public string Header { get; set; }
        public string Styles { get; set; }
        public string Scripts { get; set; }
        public string Layout { get; set; }
        public string AdditionalHeaders { get; set; }
    }
}
