using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcSiteMapProvider;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Comments.Contract;
using RoCMS.Comments.Contract.Models;
using RoCMS.Comments.Contract.Services;
using RoCMS.Comments.Contract.ViewModels;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Comments.Web.Controllers
{
    [AuthorizeResources(RoCmsResources.AdminPanel, CommentsRoCMSResources.CommentsEditor)]
    public class CommentsEditorController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IHeartService _heartService;

        public CommentsEditorController(ICommentService commentService, IHeartService heartService)
        {
            _commentService = commentService;
            _heartService = heartService;
        }

        [MvcSiteMapNode(Title = "Комментарии", ParentKey = "AdminHome", Key = "Comments", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""CommentsEditor"", ""visibility"": ""AdminMenu"", ""iconClass"" : ""fa-comments-o"" }")]
        [PagingFilter]
        public ActionResult Topics(int page = 1, int pageSize = 10)
        {
            int totalCount;
            int startIndex = (page - 1) * pageSize + 1;
            ICollection<CommentTopicVM> topics = _commentService.GetTopicVMs(startIndex, pageSize, out totalCount);
            return View(topics);
        }

        public ActionResult Topic(int id)
        {
            Heart heart = _heartService.GetHeart(id);
            ICollection<CommentVM> comments = _commentService.GetThreadsByHeart(id);
            return View(new Tuple<Heart, ICollection<CommentVM>>(heart, comments));
        }
    }
}