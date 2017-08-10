namespace RoCMS.Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Description { get; set; }
        public string Vk { get; set; }
        public string Fb { get; set; }
        public string GoogleP { get; set; }
        public string Twitter { get; set; }
    }
}
