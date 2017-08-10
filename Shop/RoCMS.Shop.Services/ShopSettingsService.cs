using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Services
{
    public class ShopSettingsService: IShopSettingsService
    {
        private ISettingsService _settingsService;
        public ShopSettingsService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public ShopSettings GetShopSettings()
        {
            decimal deliveryCost = _settingsService.GetSettings<decimal>("DeliveryCost");
            decimal selfPickupCost = _settingsService.GetSettings<decimal>("SelfPickupCost");
            string courierCities = _settingsService.GetSettings<string>("CourierCities");
            
            string shopUrl = _settingsService.GetSettings<string>("ShopUrl");
            return new ShopSettings() { DeliveryCost = deliveryCost, SelfPickupCost = selfPickupCost, ShopUrl = shopUrl, CourierCities = courierCities };
        }

        public void UpdateShopSettings(ShopSettings settings)
        {
            _settingsService.Set("DeliveryCost", settings.DeliveryCost);
            _settingsService.Set("SelfPickupCost", settings.SelfPickupCost);
            _settingsService.Set("CourierCities", settings.CourierCities);
            _settingsService.Set("ShopUrl", settings.ShopUrl);
        }
    }
}
