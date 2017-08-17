using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.Dev_InterfaceStrings)]
    public class InterfaceStringApiController: ApiController
    {
        private readonly IInterfaceStringService _interfaceStringService;
        private readonly ILogService _logService;

        public InterfaceStringApiController(IInterfaceStringService interfaceStringService, ILogService logService)
        {
            _interfaceStringService = interfaceStringService;
            _logService = logService;
        }

        [HttpGet]
        public ICollection<InterfaceString> Get()
        {
            return _interfaceStringService.GetStrings();
        }

        [HttpPost]
        public ResultModel Save(ICollection<InterfaceString> strings)
        {
            try
            {
                _interfaceStringService.SaveMany(strings);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Create(InterfaceString data)
        {
            try
            {
                var str = _interfaceStringService.GetString(data.Key);
                if (str == null)
                {
                    _interfaceStringService.Upsert(data);
                    return ResultModel.Success;
                }
                return new ResultModel(false, "Exists");
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
    }
}