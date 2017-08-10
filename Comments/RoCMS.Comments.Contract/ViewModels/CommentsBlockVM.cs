using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Comments.Contract.ViewModels
{
    public class CommentsBlockVM
    {
        public CommentsBlockVM(int? topicId, string addCommentApiUrl, string leaveCommentHeader, string sectionHeader)
        {
            TopicId = topicId;
            AddCommentApiUrl = addCommentApiUrl;
            LeaveCommentHeader = leaveCommentHeader;
            SectionHeader = sectionHeader;
        }
        public int? TopicId { get; set; }
        public string AddCommentApiUrl { get; set; }
        public string LeaveCommentHeader { get; set; }
        public string SectionHeader { get; set; }
    }
}
