using System;

namespace RoCMS.Web.Contract.Models
{
    public class ImageInfo
    {
        public DateTime CreationDate { get; set; }

        public string ImageId { get; set; }

        public long? Size { get; set; }
        public int AlbumCount { get; set; }
    }
}
