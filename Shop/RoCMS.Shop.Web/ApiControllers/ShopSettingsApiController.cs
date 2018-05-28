using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    public class ShopSettingsApiController : ApiController
    {
        private readonly IShopSettingsService _shopSettingsService;
        private readonly ISettingsService _settingsService;
        private readonly IShopCategoryService _shopCategoryService;
        private readonly IShopManufacturerService _manufacturerService;

        public ShopSettingsApiController(IShopSettingsService shopSettingsService, ISettingsService settingsService, IShopCategoryService shopCategoryService, IShopManufacturerService manufacturerService)
        {
            _shopSettingsService = shopSettingsService;
            _settingsService = settingsService;
            _shopCategoryService = shopCategoryService;
            _manufacturerService = manufacturerService;
        }
        [HttpGet]
        public ShopSettings Get()
        {
            return _shopSettingsService.GetShopSettings();
        }
        [HttpPost]
        public ResultModel Update(ShopSettings settings)
        {
            _shopSettingsService.UpdateShopSettings(settings);
            return ResultModel.Success;
        }

        public LastUsedValues GetLastUsedValues()
        {
            int? catId = _settingsService.GetSettings<int?>(SettingKey.LastGoodsCategory.ToString());
            int? manId = _settingsService.GetSettings<int?>(SettingKey.LastGoodsManufacturer.ToString());
            int? supId = _settingsService.GetSettings<int?>(SettingKey.LastGoodsSupplier.ToString());
            SortCriterion sortBy = _settingsService.GetSettings<SortCriterion>(SettingKey.LastGoodsSortBy.ToString());

            LastUsedValues values = new LastUsedValues()
            {
                LastSortBy = sortBy
            };

            if (catId.HasValue)
            {
                var cat = _shopCategoryService.CategoryExists(catId.Value)
                    ? _shopCategoryService.GetCategory(catId.Value)
                    : null;
                if (cat == null)
                {
                    _settingsService.Set<int?>(SettingKey.LastGoodsCategory.ToString(), null);
                }
                else
                {
                    values.LastCategory = new IdNamePair<int>(cat.HeartId, cat.Name);
                }
            }

            if (supId.HasValue)
            {
                var man = _manufacturerService.GetManufacturer(supId.Value);

                if (man == null)
                {
                    _settingsService.Set<int?>(SettingKey.LastGoodsManufacturer.ToString(), null);
                }
                else
                {
                    values.LastSupplier = new IdNamePair<int>(man.HeartId, man.Name);
                }
            }

            if (manId.HasValue)
            {
                var man = _manufacturerService.GetManufacturer(manId.Value);

                if (man == null)
                {
                    _settingsService.Set<int?>(SettingKey.LastGoodsManufacturer.ToString(), null);
                }
                else
                {
                    values.LastManufacturer = new IdNamePair<int>(man.HeartId, man.Name);
                }
            }


            return values;
        }

        public class LastUsedValues
        {
            public IdNamePair<int> LastCategory { get; set; }
            public IdNamePair<int> LastManufacturer { get; set; }
            public IdNamePair<int> LastSupplier { get; set; }

            public SortCriterion LastSortBy { get; set; }
        }


    }
}
