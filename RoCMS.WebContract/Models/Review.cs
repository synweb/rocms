using System;
using System.ComponentModel.DataAnnotations;

namespace RoCMS.Web.Contract.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Strings),
            ErrorMessageResourceName = "Validation_Reqiured")]
        public string Author { get; set; }

        public string City { get; set; }
               
        [RegularExpression("^([A-Za-z0-9_\\-\\.])+\\@([A-Za-z0-9_\\-\\.])+\\.([A-Za-z]{2,4})$",
            ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_WrongEmail")]
        public string Email { get; set; }

        public string VK { get; set; }

        //public string Skype { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resources.Strings),
            ErrorMessageResourceName = "Validation_Reqiured")]
        public string Text { get; set; }
        public string Response { get; set; }

        public bool Moderated { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
