namespace RoCMS.Comments.Contract.Models
{
    public class CommentTopic
    {
        public int CommentTopicId { get; set; }
        public string TargetType { get; set; }
        public int? TargetId { get; set; }
        public string TargetUrl { get; set; }
        public string TargetTitle { get; set; }
    }
}
