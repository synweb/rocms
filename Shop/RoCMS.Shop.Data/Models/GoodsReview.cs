using System;

namespace RoCMS.Shop.Data.Models
{
    public class GoodsReview
    {
        public int GoodsReviewId { get; set; }
        public int GoodsId { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public string AuthorContact { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Text { get; set; }
        public Nullable<int> UserId { get; set; }
        public bool Moderated { get; set; }
    }
}
