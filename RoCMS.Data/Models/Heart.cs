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

        public void Fill(Heart anotherHeart)
        {
            this.HeartId = anotherHeart.HeartId;
            this.CreationDate = anotherHeart.CreationDate;
            this.RelativeUrl = anotherHeart.RelativeUrl;
            this.ParentHeartId = anotherHeart.ParentHeartId;
            this.BreadcrumbsTitle = anotherHeart.BreadcrumbsTitle;
            this.Noindex = anotherHeart.Noindex;
            this.Title = anotherHeart.Title;
            this.MetaDescription = anotherHeart.MetaDescription;
            this.MetaKeywords = anotherHeart.MetaKeywords;
            this.Styles = anotherHeart.Styles;
            this.Scripts = anotherHeart.Scripts;
            this.Layout = anotherHeart.Layout;
            this.AdditionalHeaders = anotherHeart.AdditionalHeaders;
        }
    }
}
