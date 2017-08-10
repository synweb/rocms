using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopSettingsService
    {
        ShopSettings GetShopSettings();
        void UpdateShopSettings(ShopSettings settings);
    }
}
