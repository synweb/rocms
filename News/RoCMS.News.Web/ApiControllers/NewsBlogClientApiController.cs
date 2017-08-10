using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Web.ApiControllers
{
    [Authorize]
    public class NewsBlogClientApiController: ApiController
    {
        private readonly IBlogService _blogService;
        private readonly INewsItemService _newsItemService;
        private readonly ISecurityService _securityService;
        private readonly IImageService _imageService;

        public NewsBlogClientApiController(IBlogService blogService, INewsItemService newsItemService, ISecurityService securityService, IImageService imageService)
        {
            _blogService = blogService;
            _newsItemService = newsItemService;
            _securityService = securityService;
            _imageService = imageService;
        }

        [HttpPost]
        public ResultModel CreateBlog(Blog blog)
        {
            try
            {
                int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                blog.OwnerId = userId;
                _blogService.CreateBlog(blog);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }


        [HttpPost]
        public ResultModel UpdateBlog(Blog blog)
        {
            try
            {
                int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                bool canUpdate = _blogService.CheckIfUserHasAccess(userId, blog.BlogId);
                if (canUpdate)
                {
                    _blogService.UpdateBlogByClient(blog);
                    return ResultModel.Success;
                }
                else
                {
                    return new ResultModel(false, "Forbidden");
                }
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel CreatePost(PostData data)
        {
            try
            {
                int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                var blog = _blogService.GetUserBlog(userId);
                var post = new NewsItem();
                post.BlogId = blog.BlogId;
                post.AuthorId = userId;
                post.Title = data.Title;
                post.Description = data.Description;
                post.Text = data.Text;
                post.ImageId = data.ImageId;
                _newsItemService.CreateClientPost(post);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel DeletePost(int newsId)
        {
            try
            {
                int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                var item = _newsItemService.GetNewsItem(newsId);
                bool canUpdate = _blogService.CheckIfUserHasAccess(userId, item.BlogId) && !item.Categories.Any();
                if (canUpdate)
                {
                    _newsItemService.RemoveNewsItem(newsId);
                    return ResultModel.Success;
                }
                else
                {
                    return new ResultModel(false, "Forbidden");
                }
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel UpdatePost(PostData data)
        {
            try
            {
                int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                var blog = _blogService.GetUserBlog(userId);
                bool canUpdate = _blogService.CheckIfUserHasAccess(userId, blog.BlogId);
                if (canUpdate)
                {
                    var original = _newsItemService.GetNewsItem(data.NewsId);
                    if (original.Categories.Any())
                    {
                        return new ResultModel(false, "Forbidden");
                    }
                    original.Title = data.Title;
                    original.Description = data.Description;
                    original.Text = data.Text;
                    if (!original.ImageId.Equals(data.ImageId))
                    {
                        _imageService.RemoveImage(original.ImageId);
                    }
                    original.ImageId = data.ImageId;
                    _newsItemService.UpdateClientPost(original);
                    return ResultModel.Success;
                }
                else
                {
                    return new ResultModel(false, "Forbidden");
                }
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        public class PostData
        {
            public int NewsId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Text { get; set; }
            public string ImageId { get; set; }
        }

        [HttpGet]
        public HttpResponseMessage BlogUrlIsFree(string url)
        {
            int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
            var userBlog = _blogService.GetUserBlog(userId);
            bool resBool = 
                userBlog == null
                || userBlog.RelativeUrl.Equals(url)
                || !_blogService.CheckIfExists(url);
            var resStr = resBool.ToString().ToLower();
            var res = new HttpResponseMessage(HttpStatusCode.OK);
            res.Content = new StringContent(resStr);
            return res;
        }

        public ResultModel UpdateProfile(User user)
        {
            try
            {
                int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
                if (userId != user.UserId)
                {
                    return new ResultModel(false, "Нельзя редактировать чужой профиль");
                }


                _securityService.UpdateUser(user);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}