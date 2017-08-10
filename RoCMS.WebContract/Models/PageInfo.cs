using System.ComponentModel.DataAnnotations;

namespace RoCMS.Web.Contract.Models
{
    /// <summary>
    /// Сокращённый вариант страницы (без содержимого)
    /// </summary>
    public class PageInfo
    {
        public System.DateTime CreationDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_Reqiured")]
        public string Title { get; set; }

        public string Header { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_Reqiured")]
        public string Annotation { get; set; }
        
        [RegularExpression(@"[a-zA-Zа-яА-Я0-9_-]+",ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_LettersNumbersUnderscoreDash")]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_Reqiured")]
        public string RelativeUrl { get; set; }

        public string Keywords { get; set; }

        public int PageId { get; set; }

        public string Layout { get; set; }

        public int? ParentPageId { get; set; }

        public string CannonicalUrl { get; set; }

        public bool HideInSitemap { get; set; }

        public string Styles { get; set; }
        public string Scripts { get; set; }
        public string AdditionalHeaders { get; set; }
    }
}
