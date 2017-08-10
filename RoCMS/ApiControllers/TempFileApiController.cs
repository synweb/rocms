using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    public class TempFileApiController : ApiController
    {
        private readonly ITempFilesService _tempService;

        public TempFileApiController(ITempFilesService tempService)
        {
            _tempService = tempService;
        }

        [HttpPost]
        public ResultModel RemoveFile(Guid id)
        {
            _tempService.RemoveFile(id);
            return ResultModel.Success;
        }
    }
}
