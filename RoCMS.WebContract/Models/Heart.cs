using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models
{
    public abstract class Heart
    {
        public int HeartId { get; set; }
        public DateTime CreationDate { get; set; }

        [RegularExpression(@"[a-zA-Zа-яА-Я0-9_-]+", ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_LettersNumbersUnderscoreDash")]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_Reqiured")]
        public string RelativeUrl { get; set; }
        public int? ParentHeartId { get; set; }
        public string BreadcrumbsTitle { get; set; }
        public bool Noindex { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_Reqiured")]
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Styles { get; set; }
        public string Scripts { get; set; }
        public string Layout { get; set; }
        public string AdditionalHeaders { get; set; }
        public string CannonicalUrl { get; set; }
    }
}
