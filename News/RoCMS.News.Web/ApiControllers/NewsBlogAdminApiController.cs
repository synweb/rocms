using System;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;

namespace RoCMS.News.Web.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.News)]
    public class NewsBlogAdminApiController: ApiController
    {
        private readonly IBlogService _blogService;

        public NewsBlogAdminApiController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost]
        public ResultModel UpdateBlog(Blog blog)
        {
            try
            {
                _blogService.UpdateBlog(blog);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Delete(int blogId)
        {
            try
            {
                _blogService.DeleteBlog(blogId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}