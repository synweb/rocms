using System;

namespace RoCMS.Comments.Data.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int? ParentCommentId { get; set; }
        public int CommentTopicId { get; set; }
        public string Text { get; set; }
        public bool Moderated { get; set; }
        public int? AuthorId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Deleted { get; set; }

        public string Url { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
