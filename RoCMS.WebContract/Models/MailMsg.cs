using System;

namespace RoCMS.Web.Contract.Models
{
    public class MailMsg
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Receiver { get; set; }
        public string BccReceiver { get; set; }
        public Guid[] AttachIds { get; set; }
    }
}
