using System.Collections.Generic;
using RoCMS.Base.Models;
using RoCMS.Comments.Contract.Models;
using RoCMS.Comments.Contract.ViewModels;

namespace RoCMS.Comments.Contract.Services
{
    public interface ICommentService
    {
        ICollection<CommentTopic> GetTopics(int startIndex, int count, out int totalCount);
        int CreateComment(Comment comment);
        void DeleteComment(int commentId);
        void UpdateCommentText(int commentId, string text);
        void ModerateComment(int commentId, bool moderated);
        ICollection<Comment> GetCommentsByHeart(int heartId, bool? moderated = null);
        ICollection<Comment> GetCommentsByAuthor(int authorId, bool? moderated = null);
        ICollection<CommentVM> GetThreadsByHeart(int heartId, bool? moderated = null);
        Comment GetComment(int commentId);
        ICollection<CommentTopicVM> GetTopicVMs(int startIndex, int count, out int totalCount);

    }
}
