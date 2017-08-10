using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Web.ApiControllers
{
    [Authorize]
    [AuthorizeResourcesApi(RoCmsResources.News)]
    public class NewsBlogAdminApiController: ApiController
    {
        private readonly IBlogService _blogService;
        private readonly INewsItemService _newsItemService;
        private readonly ISecurityService _securityService;
        private readonly IImageService _imageService;

        public NewsBlogAdminApiController(IBlogService blogService, INewsItemService newsItemService, ISecurityService securityService, IImageService imageService)
        {
            _blogService = blogService;
            _newsItemService = newsItemService;
            _securityService = securityService;
            _imageService = imageService;
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