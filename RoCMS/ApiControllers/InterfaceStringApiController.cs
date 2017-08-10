using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    public class InterfaceStringApiController: ApiController
    {
        private readonly IInterfaceStringService _interfaceStringService;

        public InterfaceStringApiController(IInterfaceStringService interfaceStringService)
        {
            _interfaceStringService = interfaceStringService;
        }

        public ICollection<InterfaceString> Get()
        {
            return _interfaceStringService.GetStrings();
        }

        public ResultModel Save(ICollection<InterfaceString> strings)
        {
            try
            {
                _interfaceStringService.SaveMany(strings);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
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
                else
                {
                    return new ResultModel(false, "Exists");
                }
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}