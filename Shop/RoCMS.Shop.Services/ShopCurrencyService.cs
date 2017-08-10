using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Services
{
    class ShopCurrencyService: BaseShopService, IShopCurrencyService
    {
        private readonly ILogService _logService;
        private readonly CurrencyGateway _currencyGateway =new CurrencyGateway();

        public ShopCurrencyService(ILogService logService)
        {
            _logService = logService;
        }

        public IList<Currency> GetCurrencies()
        {
            var dataRes = _currencyGateway.Select();
            var res = Mapper.Map<IList<Currency>>(dataRes);
            return res;
        }

        public Currency GetCurrency(string currencyId)
        {
            var dataRes = _currencyGateway.SelectOne(currencyId);
            var res = Mapper.Map<Currency>(dataRes);
            return res;
        }

        public void CreateCurrency(Currency currency)
        {
            var dataRec = Mapper.Map<Data.Models.Currency>(currency);
            _currencyGateway.Insert(dataRec);
        }

        public void UpdateCurrency(Currency currency)
        {
            var dataRec = Mapper.Map<Data.Models.Currency>(currency);
            _currencyGateway.Update(dataRec);
        }


        public class ValCurs
        {
            [XmlElement("Valute")]
            public ShopCurrencyService.ValCursValute[] ValuteList;
        }

        public class ValCursValute
        {

            [XmlElement("CharCode")]
            public string ValuteStringCode;

            [XmlElement("Name")]
            public string ValuteName;

            [XmlElement("Value")]
            public string ExchangeRate;

            [XmlElement("Nominal")]
            public string Nominal;
        }


        public class CurrencyRate
        {
            /// <summary>
            /// Закодированное строковое обозначение валюты
            /// Например: USD, EUR, AUD и т.д.
            /// </summary>
            public string CurrencyStringCode;

            /// <summary>
            /// Обменный курс
            /// </summary>
            public decimal ExchangeRate;
        }

        public void UpdateFromCentralBank()
        {
            try
            {
                List<ShopCurrencyService.CurrencyRate> cbData = new List<ShopCurrencyService.CurrencyRate>();
                XmlSerializer xs = new XmlSerializer(typeof(ShopCurrencyService.ValCurs));
                XmlReader xr = new XmlTextReader(@"http://www.cbr.ru/scripts/XML_daily.asp");
                foreach (ShopCurrencyService.ValCursValute valute in ((ShopCurrencyService.ValCurs)xs.Deserialize(xr)).ValuteList)
                {
                    cbData.Add(new ShopCurrencyService.CurrencyRate()
                    {
                        CurrencyStringCode = valute.ValuteStringCode,
                        ExchangeRate = System.Convert.ToDecimal(valute.ExchangeRate, new CultureInfo("ru-RU")) / System.Convert.ToInt32(valute.Nominal)
                    });
                }
                decimal modifier = 1.02m; // Курс ЦБ + 2%
                using (var ts = new TransactionScope())
                {
                    var dataCurrs = _currencyGateway.Select();
                    foreach (var currency in dataCurrs)
                    {
                        var rec = cbData.SingleOrDefault(x => x.CurrencyStringCode == currency.CurrencyId);
                        if (rec == null)
                            continue;
                        currency.Rate = rec.ExchangeRate * modifier;
                        _currencyGateway.Update(currency);
                    }
                    ts.Complete();
                }
                _logService.TraceMessage("Курсы валют импортированы");
            }
            catch (Exception e)
            {
                _logService.LogError(e, "Ошибка при импорте курсов валют");
            }
        }

        public void DeleteCurrency(string currencyId)
        {
            _currencyGateway.Delete(currencyId);
        }

        public decimal Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            var from = _currencyGateway.SelectOne(fromCurrency);
            var to = _currencyGateway.SelectOne(toCurrency);
            decimal result = Math.Ceiling(amount/to.Rate*from.Rate);
            return result;
        }

        public decimal ConvertToDisplayCurrency(decimal amount, string fromCurrency)
        {
            string cur = GetDisplayCurrency().CurrencyId;
            return Convert(amount, fromCurrency, cur);
        }

        public Currency GetDisplayCurrency()
        {
            var context = HttpContext.Current;
            var cookie = context.Request.Cookies["defaultCurrency"];
            return GetCurrency(cookie != null ? cookie.Value : "RUR");
        }
    }
}
