using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    public class ShopSettingsApiController : ApiController
    {
        private readonly IShopSettingsService _settingsService;

        public ShopSettingsApiController(IShopSettingsService settingsService)
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
