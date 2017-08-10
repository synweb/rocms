namespace RoCMS.Data.Models
{
    public class Slide
    {
        public int SlideId { get; set; }
        public int SliderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageId { get; set; }
        public string Link { get; set; }
        public int SortOrder { get; set; }
    }
}
