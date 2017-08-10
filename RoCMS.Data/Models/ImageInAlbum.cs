namespace RoCMS.Data.Models
{
    public class ImageInAlbum
    {
        public int AlbumId { get; set; }
        public string ImageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public string DestinationUrl { get; set; }
    }
}
