using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopCurrencyService
    {
        IList<Currency> GetCurrencies();
        Currency GetCurrency(string currencyId);
        void CreateCurrency(Currency currency);
        void UpdateCurrency(Currency currency);
        void UpdateFromCentralBank();
        void DeleteCurrency(string currencyId);
        decimal Convert(decimal amount, string fromCurrency, string toCurrency);
        decimal ConvertToDisplayCurrency(decimal amount, string fromCurrency);
        Currency GetDisplayCurrency();
    }
}
