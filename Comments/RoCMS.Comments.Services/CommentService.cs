using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Models;
using RoCMS.Comments.Contract;
using RoCMS.Comments.Contract.Models;
using RoCMS.Comments.Contract.Services;
using RoCMS.Comments.Contract.ViewModels;
using RoCMS.Comments.Data.Gateways;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Comments.Services
{
    public class CommentService : BaseService, ICommentService
    {
        private readonly IHeartService _heartService;
        private readonly CommentGateway _commentGateway = new CommentGateway();
        private readonly ISettingsService _settingsService;
        private readonly ISecurityService _securityService;
        private readonly ILogService _logService;
        private readonly bool _premoderation;


        protected override int CacheExpirationInMinutes
        {
            get { throw new NotImplementedException(); }
        }

        public CommentService(IMapperService mapper, ISettingsService settingsService, ISecurityService securityService,
            ILogService logService, IHeartService heartService) : base(mapper)
        {
            _settingsService = settingsService;
            _securityService = securityService;
            _logService = logService;
            _heartService = heartService;

            // TODO: перенесено сюда для того, чтобы транзакции EF и EL не пересекались.
            // Убрать, когда переведём _settingsService на EL
            try
            {
                _premoderation = _settingsService.GetSettings<bool>(CommentsSettingStrings.CommentsPremoderation);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                _premoderation = true;
            }
        }





        public int CreateComment(Comment comment)
        {
            var dataComment = _mapper.Map<Data.Models.Comment>(comment);
            bool premoderation = _premoderation;
            dataComment.Moderated = !premoderation;
            int res = _commentGateway.Insert(dataComment);
            return res;
        }

        public void DeleteComment(int commentId)
        {
            _commentGateway.Delete(commentId);
        }

        public void UpdateCommentText(int commentId, string text)
        {
            _commentGateway.UpdateText(commentId, text);
        }

        public void ModerateComment(int commentId, bool moderated)
        {
            _commentGateway.UpdateModerated(commentId, moderated);
        }

        public ICollection<Comment> GetCommentsByHeart(int topicId, bool? moderated = null)
        {
            var dataRes = _commentGateway.SelectByHeart(topicId, moderated);
            var res = _mapper.Map<ICollection<Comment>>(dataRes);
            return res;
        }

        public ICollection<Comment> GetCommentsByAuthor(int authorId, bool? moderated = null)
        {
            var dataRes = _commentGateway.SelectByAuthor(authorId, moderated);
            var res = _mapper.Map<ICollection<Comment>>(dataRes);
            return res;
        }

        public ICollection<CommentVM> GetThreadsByHeart(int heartId, bool? moderated = null)
        {
            var res = new List<CommentVM>();
            var comments = GetCommentsByHeart(heartId, moderated).OrderBy(x => x.CreationDate);
            foreach (var comment in comments)
            {
                var newRec = CreateVM(comment);
                if (!newRec.ParentCommentId.HasValue)
                {
                    res.Add(newRec);
                }
                else
                {
                    var parentRec = FindNode(res, newRec.ParentCommentId.Value);
                    parentRec.Replies.Add(newRec);
                }
            }
            return res;
        }

        private CommentVM CreateVM(Comment comment)
        {
            var res = _mapper.Map<CommentVM>(comment);
            if (comment.AuthorId.HasValue)
            {
                res.Author = _securityService.GetUsername(comment.AuthorId.Value);

                if (String.IsNullOrWhiteSpace(comment.Name))
                {
                    comment.Name = res.Author;
                }
                if (String.IsNullOrWhiteSpace(comment.Email))
                {
                    comment.Email = res.Email;
                }
                if (String.IsNullOrWhiteSpace(comment.Url))
                {
                    comment.Name = res.Url;
                }
            }
            return res;
        }

        public Comment GetComment(int commentId)
        {
            var dataRes = _commentGateway.SelectOne(commentId);
            var res = _mapper.Map<Comment>(dataRes);
            return res;
        }

        public ICollection<CommentTopicVM> GetTopicVMs(int startIndex, int count, out int totalCount)
        {
            var res = new List<CommentTopicVM>();
            var topics = GetTopics(startIndex, count, out totalCount);
            foreach (var commentTopic in topics)
            {
                var newVm = _mapper.Map<CommentTopicVM>(commentTopic);
                newVm.CommentCount = _commentGateway.SelectCount(newVm.HeartId);
                res.Add(newVm);
            }
            return res;
        }


        public ICollection<CommentTopic> GetTopics(int startIndex, int count, out int totalCount)
        {
            var heartIds = _commentGateway.SelectHeartIds(startIndex, count, out totalCount);
            var hearts = _heartService.GetHearts(heartIds);
            var res = _mapper.Map<ICollection<CommentTopic>>(hearts);
            return res;
        }

        /// <summary>
        /// Поиск элемента в дереве комментов
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        private CommentVM FindNode(CommentVM tree, int nodeId)
        {
            CommentVM res = FindNodeInner(tree, nodeId);
            if (res == null)
            {
                throw new KeyNotFoundException();
            }
            return res;
        }

        private CommentVM FindNode(IEnumerable<CommentVM> trees, int nodeId)
        {
            CommentVM res = null;
            foreach (var tree in trees)
            {
                res = FindNodeInner(tree, nodeId);
                if (res != null)
                {
                    break;
                }
            }
            if (res == null)
            {
                throw new KeyNotFoundException();
            }
            return res;
        }

        /// <summary>
        /// Поиск элемента в дереве комментов, может возвращать NULL
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        private CommentVM FindNodeInner(CommentVM tree, int nodeId)
        {
            if (tree.CommentId == nodeId)
            {
                return tree;
            }
            CommentVM res = null;
            foreach (var reply in tree.Replies)
            {
                res = FindNodeInner(reply, nodeId);
                if (res != null)
                {
                    break;
                }
            }
            return res;
        }
    }
}
