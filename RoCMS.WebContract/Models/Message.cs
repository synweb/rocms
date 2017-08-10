using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RoCMS.Web.Contract.Resources;

namespace RoCMS.Web.Contract.Models
{
    public class Message
    {
        public Message()
        {
            Fields = new Dictionary<string, string>();
        }
        public MessageType MessageType { get; set; }

        public string Name { get; set; }

        //[CustomValidation(typeof(MessageContactsValidator), "Validate")]
        [RegularExpression("^([A-Za-z0-9_\\-\\.])+\\@([A-Za-z0-9_\\-\\.])+\\.([A-Za-z]{2,4})$",
            ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_WrongEmail")]
        public string Email { get; set; }

        //[CustomValidation(typeof(MessageContactsValidator), "Validate")]
        public string Phone { get; set; }

        /// <summary>
        /// Контактные данные заказчика
        /// </summary>
        public string Contact { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Strings), ErrorMessageResourceName = "Validation_Reqiured")]
        public string Text { get; set; }

        public Guid[] AttachIds { get; set; }

        public int? OrderFormId { get; set; }

        public Dictionary<string, string> Fields { get; set; }
    }

    public enum MessageType 
    {
        Order,
        //ShopOrder,
        //Question,
        //Suggestion,
        CallMeBack
    }

    public static class MessageContactsValidator
    {
        public static ValidationResult Validate(string value, ValidationContext context)
        {
            var message = context.ObjectInstance as Message;
            if(message == null) throw new ApplicationException("Валидатор может применяться только к типу RoCMS.Web.Contract.Models.Message");

            if(String.IsNullOrWhiteSpace(message.Phone) && String.IsNullOrWhiteSpace(message.Email))
            {
                return new ValidationResult(Strings.Validation_MessageContactRequired);
            }
            return ValidationResult.Success;
        }
    }
}
