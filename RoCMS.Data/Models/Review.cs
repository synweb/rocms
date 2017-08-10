using System;

namespace RoCMS.Data.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public string Author { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Text { get; set; }
        public string Response { get; set; }

        public bool Moderated { get; set; }

        public string VK { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
