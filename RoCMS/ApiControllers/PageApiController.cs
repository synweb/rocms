﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{

    public class PageApiController : ApiController
    {
        private readonly IPageService _pageService;
        public PageApiController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [AuthorizeResourcesApi(RoCmsResources.Pages)]
        [System.Web.Http.HttpGet]
        public IList<PageInfo> Pages()
        {
            IList<PageInfo> pages = _pageService.GetPagesInfo();
            return pages;
        }

        [AuthorizeResourcesApi(RoCmsResources.Pages)]
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
                return new ResultModel(e);
            }
        }

        [AuthorizeResourcesApi(RoCmsResources.Pages)]
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
                page.RelativeUrl = _pageService.GetNextAvailableRelativeUrl(page.RelativeUrl);
                int newId = _pageService.CreatePage(page);
                return new ResultModel(true, new {id=newId, relativeUrl = page.RelativeUrl});
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}
