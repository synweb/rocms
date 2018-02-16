namespace RoCMS.Shop.Contract.Models
{
    public class GoodsReview
    {
        public int GoodsReviewId { get; set; }
        public int HeartId { get; set; }
        public GoodsItem GoodsItem { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public string AuthorContact { get; set; }
        public int? Rating { get; set; }
        public string Text { get; set; }
        public bool Moderated { get; set; }
        public int? UserId { get; set; }
    }
}
