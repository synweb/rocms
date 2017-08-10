using System;

namespace RoCMS.Web.Contract.Models
{
    public class TempFile
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string IpAddress { get; set; }
        public byte[] Content { get; set; }
    }
}
