using RoCMS.Base.Models;
using RoCMS.News.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RoCMS.News.Web.ApiControllers
{
    public class TagApiController : ApiController
    {
        private readonly INewsItemService _newsItemService;       

        public TagApiController(INewsItemService newsItemService)
        {           
            _newsItemService = newsItemService;           
        }

        [HttpGet]
        public string[] GetTagByPattern(string term, int records = 10)
        {
            var tags = _newsItemService.GetTagByPattern(term, records).ToArray();
            return tags;
        }
    }
}
