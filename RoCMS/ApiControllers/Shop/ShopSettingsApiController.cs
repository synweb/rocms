using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models.Shop;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers.Shop
{
    public class ShopSettingsApiController : ApiController
    {
        private ISettingsService _settingsService;

        public ShopSettingsApiController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        [HttpGet]
        public ShopSettings Get()
        {
            return _settingsService.GetShopSettings();
        }
        [HttpPost]
        public ResultModel Update(ShopSettings settings)
        {
            _settingsService.UpdateShopSettings(settings);
            return ResultModel.Success;
        }
    }
}
