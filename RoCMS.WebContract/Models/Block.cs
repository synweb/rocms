using System.ComponentModel.DataAnnotations;

namespace RoCMS.Web.Contract.Models
{
    public class Block
    {
        public int BlockId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        [StringLength(200)]
        [RegularExpression(@"^(([a-zA-Zа-яА-ЯёЁ]{1})|([a-zA-Zа-яА-ЯёЁ]{1}[a-zA-Zа-яА-ЯёЁ]{1})|([a-zA-Zа-яА-ЯёЁ]{1}[0-9]{1})|([0-9]{1}[a-zA-Zа-яА-ЯёЁ]{1})|([a-zA-Zа-яА-ЯёЁ0-9][a-zA-Zа-яА-ЯёЁ0-9-_]{1,195}[a-zA-Zа-яА-ЯёЁ0-9]))$",
            ErrorMessage = "Неверный формат")]
        public string Name { get; set; }
    }
}
