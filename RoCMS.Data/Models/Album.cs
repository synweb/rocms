using System;

namespace RoCMS.Data.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string WatermarkImageId { get; set; }
    }
}
