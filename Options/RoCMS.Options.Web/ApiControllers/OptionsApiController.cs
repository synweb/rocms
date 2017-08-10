using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Options.Contract;
using RoCMS.Options.Contract.Models;
using RoCMS.Options.Contract.Services;

namespace RoCMS.Options.Web.ApiControllers
{
    [AuthorizeResourcesApi(OptionsRoCmsResources.Options)]
    public class OptionsApiController : ApiController
    {
        private IOptionService _optionService;

        public OptionsApiController(IOptionService optionService)
        {
            _optionService = optionService;
        }
        
        [System.Web.Http.HttpGet]
        public IList<OptionKey> GetOptions()
        {
            return _optionService.GetOptions();
        }
        [System.Web.Http.HttpGet]
        public OptionKey Get(int id)
        {
            return _optionService.GetOption(id);
        }

        [System.Web.Http.HttpPost]
        public ResultModel Remove(int id)
        {
            _optionService.RemoveOption(id);
            return ResultModel.Success;
        }
        [System.Web.Http.HttpPost]
        public ResultModel Create(OptionKey option)
        {
            int id = _optionService.CreateOption(option);
            return new ResultModel(true, id);
        }
        [System.Web.Http.HttpPost]
        public ResultModel Update(OptionKey option)
        {
            _optionService.UpdateOption(option);
            return ResultModel.Success;
        }
    }
}
