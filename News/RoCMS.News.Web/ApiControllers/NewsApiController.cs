using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.Web.Contract.Extensions;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Web.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.News)]
    public class NewsApiController : ApiController
    {
        private readonly ISearchService _searchService;
        private readonly ISecurityService _securityService;

        private readonly INewsItemService _newsItemService;
        private readonly ILogService _logService;
        
        public NewsApiController(ISecurityService securityService, INewsItemService newsItemService, ISearchService searchService, ILogService logService)
        {
            _securityService = securityService;
            _newsItemService = newsItemService;
            _searchService = searchService;
            _logService = logService;
        }

        [HttpGet]
        public ResultModel Get(int id)
        {
            try
            {
                var res = _newsItemService.GetNewsItem(id);

                res.PostingDate =  res.PostingDate.ApplySiteTimezone();
                res.EventDate = res.EventDate?.ApplySiteTimezone();

                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpGet]
        public ResultModel GetNewsItems()
        {
            try
            {
                var res = _newsItemService.GetAllNews();
                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel IncreaseViewCount(int id)
        {
            try
            {
                if (!_newsItemService.NewsItemExists(id))
                    return new ResultModel(false, "NotExists");

                const string NEWS_COUNTER_SESSION_KEY_FORMAT = "NewsItem_ViewCount_{0}";
                string sessionKey = string.Format(NEWS_COUNTER_SESSION_KEY_FORMAT, id);
                var sessionValue = HttpContext.Current.Session[sessionKey];
                // если значение в сессии существует, считаем, что посетитель уже увеличил счётчик
                if (sessionValue != null)
                    return new ResultModel(false, "AlreadyDone");

                _newsItemService.IncreaseViewCount(id);
                HttpContext.Current.Session[sessionKey] = true;
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Create(NewsItem news)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResultModel(false);
                }
                if (!string.IsNullOrWhiteSpace(news.Tags) && !ValidationHelper.ValidateTags(news.Tags))
                {
                    return new ResultModel(false) { ErrorType = "Tags" };
                }
                news.CreationDate = DateTime.UtcNow;
                string username = User.Identity.Name;
                int userId = _securityService.GetUser(username).UserId;
                news.AuthorId = userId;
                if (String.IsNullOrWhiteSpace(news.ImageId))
                {
                    news.ImageId = null;
                }
                int id = _newsItemService.CreateNewsItem(news);
                news.HeartId = id;
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }

        }



        [HttpPost]
        public ResultModel Update(NewsItem news)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return new ResultModel(false);
                }
                if (!string.IsNullOrWhiteSpace(news.Tags) && !ValidationHelper.ValidateTags(news.Tags))
                {
                    return new ResultModel(false) { ErrorType = "Tags" };
                }
                if (String.IsNullOrWhiteSpace(news.ImageId))
                {
                    news.ImageId = null;
                }
                _newsItemService.UpdateNewsItem(news);
                
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Delete(int id)
        {
            try
            {
                _newsItemService.RemoveNewsItem(id);
                
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public ResultModel CreateComment(int id, Comment comment)
        //{
        //    try
        //    {
        //        if (String.IsNullOrWhiteSpace(comment.Text))
        //        {
        //            return new ResultModel(false) { ErrorType = "TextEmpty" };
        //        }
        //        comment.AuthorId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
        //        int res = _newsItemService.CreateComment(id, comment);
        //        return new ResultModel(true, res);
        //    }
        //    catch (Exception e)
        //    {
        //        _logService.LogError(e);
        //        return new ResultModel(e);
        //    }
        //}

        //[Authorize]
        //[HttpPost]
        //public ResultModel DeleteComment(int id)
        //{
        //    try
        //    {
        //        int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
        //        var comment = _newsItemService.GetComment(id);
        //        if (comment.AuthorId != userId)
        //        {
        //            return new ResultModel(false, "Forbidden");
        //        }
        //        _newsItemService.DeleteComment(id);
        //        return ResultModel.Success;
        //    }
        //    catch (Exception e)
        //    {
        //        _logService.LogError(e);
        //        return new ResultModel(e);
        //    }
        //}

        //[AuthorizeResources(RoCmsResources.AdminPanel, RoCmsResources.News, CommentsRoCMSResources.CommentsEditor)]
        //[HttpPost]
        //public ResultModel AdminDeleteComment(int id)
        //{
        //    try
        //    {
        //        _newsItemService.DeleteComment(id);
        //        return ResultModel.Success;
        //    }
        //    catch (Exception e)
        //    {
        //        _logService.LogError(e);
        //        return new ResultModel(e);
        //    }
        //}

        [AllowAnonymous]
        [HttpGet]
        public ResultModel Filter(string cats=null, string query = null, int page = 1, int pgsize = int.MaxValue)
        {
            try
            {
                int count;
                var news = _newsItemService.GetNewsPage(new NewsFilter() {CategoryQuery = cats, TextQuery = query}, page, pgsize, out count);
                var resultIds = news.Select(x => x.HeartId);
                return new ResultModel(true, new { resultIds, count });
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
    }
}