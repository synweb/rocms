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

namespace RoCMS.Comments.Web.Controllers
{
    [AuthorizeResources(RoCmsResources.AdminPanel, CommentsRoCMSResources.CommentsEditor)]
    public class CommentsEditorController: Controller
    {
        private readonly ICommentService _commentService;

        public CommentsEditorController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [MvcSiteMapNode(Title = "Комментарии", ParentKey = "AdminHome", Key = "Comments", VisibilityProvider = "RoCMS.Helpers.RoCMSSiteMapNodesVisibilityProvider, RoCMS", Attributes = @"{ ""cmsResourceRequired"":""CommentsEditor"", ""visibility"": ""AdminMenu"", ""iconClass"" : ""fa-comments-o"" }")]
        public ActionResult Topics(PagingFilter paging)
        {
            int totalCount;
            ICollection<CommentTopicVM> topics = _commentService.GetTopicVMs(paging, out totalCount);
            return View(topics);
        }

        public ActionResult Topic(int id)
        {
            CommentTopic topic = _commentService.GetTopic(id);
            ICollection<CommentVM> comments = _commentService.GetThreadsByTopic(id, false);
            return View(new Tuple<CommentTopic, ICollection<CommentVM>>(topic, comments));
        }
    }
}