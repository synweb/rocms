using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{

    [AuthorizeResourcesApi(RoCmsResources.Pages)]
    public class PageApiController : ApiController
    {
        private readonly IPageService _pageService;
        private readonly IHeartService _heartService;
        private readonly ILogService _logService;

        public PageApiController(IPageService pageService, ILogService logService, IHeartService heartService)
        {
            _pageService = pageService;
            _logService = logService;
            _heartService = heartService;
        }

        [System.Web.Http.HttpGet]
        public IList<Page> Pages()
        {
            return _pageService.GetPages();
        }

        public ResultModel GetPage(string relativeUrl)
        {
            try
            {
                var page = _pageService.GetPage(relativeUrl);
                return new ResultModel(true, page);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel CreatePage(Page page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResultModel(false);
                }

                _pageService.CreatePage(page);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdatePage(Page page)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResultModel(false);
                }

                _pageService.UpdatePage(page);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel CopyPage(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ResultModel(false);
                }
                var page = _pageService.GetPage(id);
                page.RelativeUrl = _heartService.GetNextAvailableRelativeUrl(page.RelativeUrl);
                int newId = _pageService.CreatePage(page);
                return new ResultModel(true, new {id=newId, relativeUrl = page.RelativeUrl});
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
    }
}
