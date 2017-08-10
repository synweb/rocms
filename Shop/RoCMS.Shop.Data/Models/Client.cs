using System;

namespace RoCMS.Shop.Data.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool EmailNotificationAllowed { get; set; }
        public bool SmsNotificationAllowed { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string Address { get; set; }
    }
}
