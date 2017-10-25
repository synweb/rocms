using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Comments.Contract.Models
{
    public class CommentTopic
    {
        public int HeartId { get; set; }
        public string TargetCanonicalUrl { get; set; }
        public string TargetTitle { get; set; }
    }
}
