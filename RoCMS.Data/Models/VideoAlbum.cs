using System;

namespace RoCMS.Data.Models
{
    public class VideoAlbum
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
        public int OwnerId { get; set; }
    }
}
