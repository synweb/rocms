using System.Collections.Generic;
using RoCMS.Base.Models;
using RoCMS.Comments.Contract.Models;
using RoCMS.Comments.Contract.ViewModels;

namespace RoCMS.Comments.Contract.Services
{
    public interface ICommentService
    {
        int CreateTopic(CommentTopic topic);
        void RemoveTopic(int topicId);
        ICollection<CommentTopic> GetTopics(PagingFilter paging, out int totalCount);
        int CreateComment(Comment comment);
        void DeleteComment(int commentId);
        void UpdateCommentText(int commentId, string text);
        void ModerateComment(int commentId, bool moderated);
        ICollection<Comment> GetCommentsByTopic(int topicId, bool onlyModerated);
        ICollection<Comment> GetCommentsByAuthor(int authorId, bool onlyModerated);
        ICollection<CommentVM> GetThreadsByTopic(int topicId, bool moderated);
        Comment GetComment(int commentId);
        ICollection<CommentTopicVM> GetTopicVMs(PagingFilter paging, out int totalCount);
        CommentTopic GetTopic(int id);
    }
}
