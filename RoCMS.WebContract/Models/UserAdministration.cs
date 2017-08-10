using System.Collections.Generic;

namespace RoCMS.Web.Contract.Models
{
    public class UserAdministration
    {
        public IEnumerable<string> Usernames { get; set; }
        public bool AllowRegistration { get; set; }
    }
}