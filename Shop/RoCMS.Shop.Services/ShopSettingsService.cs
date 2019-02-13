using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Services
{
    public class ShopSettingsService : IShopSettingsService
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
            int defaultPageSize = _settingsService.GetSettings<int>("DefaultPageSize");

            string specsInFilterString = _settingsService.GetSettings<string>("Shop_SpecsInFilter");

            List<IdNamePair<int>> specsInFilter = new List<IdNamePair<int>>();

            try
            {
                if (!String.IsNullOrEmpty(specsInFilterString))
                {
                    var specService = DependencyResolver.Current.GetService<IShopSpecService>();
                    var specIds = specsInFilterString.Split(',').Select(x => Int32.Parse(x.Trim()));

                    var specs = specService.GetSpecs();
                    foreach (var specId in specIds)
                    {
                        var spec = specs.FirstOrDefault(x => x.SpecId == specId);
                        if (spec != null)
                        {
                            specsInFilter.Add(new IdNamePair<int>(spec.SpecId, spec.Name));
                        }
                    }

                }
            }
            catch
            {
                
            }


            return new ShopSettings()
            {
                DeliveryCost = deliveryCost,
                SelfPickupCost = selfPickupCost,
                ShopUrl = shopUrl,
                CourierCities = courierCities,
                DefaultPageSize = defaultPageSize,
                SpecsInFilter = specsInFilter

            };
        }

        public void UpdateShopSettings(ShopSettings settings)
        {
            _settingsService.Set("DeliveryCost", settings.DeliveryCost);
            _settingsService.Set("SelfPickupCost", settings.SelfPickupCost);
            _settingsService.Set("CourierCities", settings.CourierCities);
            _settingsService.Set("ShopUrl", settings.ShopUrl);
            _settingsService.Set("DefaultPageSize", settings.DefaultPageSize);

            if (settings.SpecsInFilter.Any())
            {
                string specsInFilterString = String.Join(",", settings.SpecsInFilter.Select(x => x.ID));
                _settingsService.Set("Shop_SpecsInFilter", specsInFilterString);
            }
            else
            {
                _settingsService.Set("Shop_SpecsInFilter", "");
            }
        }
    }
}
