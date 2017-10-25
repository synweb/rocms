using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Comments.Contract;
using RoCMS.Comments.Contract.Models;
using RoCMS.Comments.Contract.Services;
using RoCMS.Helpers;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Comments.Web.ApiControllers
{

    [AuthorizeResourcesApi(RoCmsResources.AdminPanel, CommentsRoCMSResources.CommentsEditor)]
    public class CommentApiController : ApiController
    {
        private readonly ICommentService _commentService;
        private readonly ILogService _logService;

        public CommentApiController(ICommentService commentService, ILogService logService)
        {
            _commentService = commentService;
            _logService = logService;
        }

        //private int UserId { get { return AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current); } }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        public ResultModel Create(Comment comment)
        {
            //    if (String.IsNullOrWhiteSpace(comment.Text))
            //    {
            //        return new ResultModel(false) {ErrorType = "TextEmpty"};
            //    }
            //    comment.AuthorId = UserId;
            //    int id = _commentService.CreateComment(comment);
            //    return new ResultModel(true, id);

            try
            {
                if (String.IsNullOrWhiteSpace(comment.Text))
                {
                    return new ResultModel(false) { ErrorType = "TextEmpty" };
                }
                //TODO: сделать нормальную настройку для доступа комментов для зарегистррованных
                //comment.AuthorId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                int res = _commentService.CreateComment(comment);
                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }


        }


        [System.Web.Http.HttpPost]
        public ResultModel Delete(int id)
        {
            try
            {
                _commentService.DeleteComment(id);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel Hide(int id)
        {
            try
            {
                _commentService.ModerateComment(id, false);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel Show(int id)
        {
            try
            {
                _commentService.ModerateComment(id, true);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}