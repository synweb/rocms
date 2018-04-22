using RoCMS.Web.Contract.Models;

namespace RoCMS.Shop.Contract.Models
{
    public class Client
    {
        public Client()
        {
            Address = new Address();
        }

        public int ClientId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool EmailNotificationAllowed { get; set; }
        public bool SmsNotificationAllowed { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }

        public string LastName { get; set; }

        //TODO: пробросить в базу
        public string SecondName { get; set; }

        public string Comment { get; set; }

        public Address Address { get; set; }
        public decimal InitialAmount { get; set; }
    }
}
