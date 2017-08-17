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
        private readonly ILogService _logService;

        public TempFileApiController(ITempFilesService tempService, ILogService logService)
        {
            _tempService = tempService;
            _logService = logService;
        }

        [HttpPost]
        public ResultModel RemoveFile(Guid id)
        {
            try
            {

                _tempService.RemoveFile(id);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
    }
}
