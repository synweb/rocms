namespace RoCMS.Web.Contract.Models
{
    public class Image: ImageInfo
    {
        public byte[] Content { get; set; }

        public string ContentType { get; set; }
    }
}
