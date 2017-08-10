using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models
{
    public class Mail
    {
        public int MailId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Receiver { get; set; }
        public string ErrorMessage { get; set; }
        public bool Sent { get; set; }
        public string Attaches { get; set; }
    }
}
