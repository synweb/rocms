using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Comments.Data.Models
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
