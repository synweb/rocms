using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Comments.Contract.ViewModels
{
    public class CommentsBlockVM
    {
        public CommentsBlockVM(int heartId, string leaveCommentHeader, string sectionHeader)
        {
            HeartId = heartId;
            LeaveCommentHeader = leaveCommentHeader;
            SectionHeader = sectionHeader;
        }
        public int HeartId { get; set; }
        public string LeaveCommentHeader { get; set; }
        public string SectionHeader { get; set; }
    }
}
