using System;
using System.Collections.Generic;
//using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models.Shop;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers.Shop
{
    public class SpecApiController : System.Web.Http.ApiController
    {
        private readonly IShopService _shopService;
        public SpecApiController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public IList<Spec> GetSpecs()
        {
            var res = _shopService.GetSpecs();
            return res;
        }

        [HttpPost]
        public ResultModel Create(Spec spec)
        {
            try
            {
                int res = _shopService.CreateSpec(spec);
                return new ResultModel(true, new {id = res});
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Update(Spec spec)
        {
            try
            {
                _shopService.UpdateSpec(spec);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel Delete(int specId)
        {
            _shopService.DeleteSpec(specId);
            return ResultModel.Success;
        }
    }
}
