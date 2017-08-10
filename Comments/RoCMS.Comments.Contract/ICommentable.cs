using System.Collections.Generic;
using RoCMS.Comments.Contract.Models;
using RoCMS.Comments.Contract.ViewModels;

namespace RoCMS.Comments.Contract
{
    public interface ICommentable
    {
        int CreateComment(int targetId, Comment comment);
        void DeleteComment(int commentId);
        void UpdateCommentText(int commentId, string text);
        void ModerateComment(int commentId, bool moderated);
        ICollection<CommentVM> GetThread(int targetId);
        Comment GetComment(int commentId);
    }
}
