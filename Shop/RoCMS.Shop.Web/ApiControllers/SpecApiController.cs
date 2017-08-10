//using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    public class SpecApiController : System.Web.Http.ApiController
    {
        private readonly IShopSpecService _shopSpecService;
        public SpecApiController(IShopSpecService shopSpecService)
        {
            _shopSpecService = shopSpecService;
        }

        [HttpGet]
        public IList<Spec> GetSpecs()
        {
            var res = _shopSpecService.GetSpecs();
            return res;
        }

        [HttpPost]
        public ResultModel Create(Spec spec)
        {
            try
            {
                int res = _shopSpecService.CreateSpec(spec);
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
                _shopSpecService.UpdateSpec(spec);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel UpdateOrder(ICollection<int> specIds)
        {
            try
            {
                _shopSpecService.UpdateSpecOrder(specIds);
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
            _shopSpecService.DeleteSpec(specId);
            return ResultModel.Success;
        }
    }
}
