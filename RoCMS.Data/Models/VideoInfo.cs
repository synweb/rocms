using System;

namespace RoCMS.Data.Models
{
    public class VideoInfo
    {
        //RoCMS.Web.Contract.Models
        public DateTime CreationDate { get; set; }

        public string VideoId { get; set; }

        public string ImageId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        //New
        public int AlbumId { get; set; }

        public int SortOrder { get; set; }
    }
}
