using RoCMS.Base.Models;
using RoCMS.News.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Web.Contract.Services;

namespace RoCMS.News.Web.ApiControllers
{
    public class TagApiController : ApiController
    {
        private readonly INewsItemService _newsItemService;
        private readonly ILogService _logService;

        public TagApiController(INewsItemService newsItemService, ILogService logService)
        {
            _newsItemService = newsItemService;
            _logService = logService;
        }

        [HttpGet]
        public string[] GetTagByPattern(string term, int records = 10)
        {
            var tags = _newsItemService.GetTagByPattern(term, records).ToArray();
            return tags;
        }
    }
}
