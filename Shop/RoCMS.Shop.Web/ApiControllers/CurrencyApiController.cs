using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    public class CurrencyApiController: ApiController
    {
        private readonly IShopCurrencyService _currencyService;

        public CurrencyApiController(IShopCurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [AuthorizeResourcesApi(ShopRoCmsResources.Shop_Currencies)]
        [HttpGet]
        public ResultModel GetCurrencies()
        {
            try
            {
                IList<Currency> res = _currencyService.GetCurrencies();
                return new ResultModel(true, res);
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [AuthorizeResourcesApi(ShopRoCmsResources.Shop_Currencies)]
        [HttpPost]
        public ResultModel CreateCurrency(Currency currency)
        {
            try
            {
                if(!Regex.IsMatch(currency.CurrencyId, "^[A-Z]+$"))
                    return new ResultModel(false, "CurrencyId");
                _currencyService.CreateCurrency(currency);
                return new ResultModel(true);
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [AuthorizeResourcesApi(ShopRoCmsResources.Shop_Currencies)]
        [HttpPost]
        public ResultModel UpdateCurrency(Currency currency)
        {
            try
            {
                if (!Regex.IsMatch(currency.CurrencyId, "^[A-Z]+$"))
                    return new ResultModel(false, "CurrencyId");
                _currencyService.UpdateCurrency(currency);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [AuthorizeResourcesApi(ShopRoCmsResources.Shop_Currencies)]
        [HttpPost]
        public ResultModel DeleteCurrency(string currencyId)
        {
            try
            {
                _currencyService.DeleteCurrency(currencyId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage SetDefaultCurrency(string id)
        {
            var currency = _currencyService.GetCurrency(id);
            if (currency == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var cookie = new CookieHeaderValue("defaultCurrency", id);
            cookie.Expires = DateTimeOffset.Now.AddYears(10);
            cookie.Domain = Request.RequestUri.Host; // домен куки
            cookie.Path = "/"; // путь куки
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return response;
        }
    }
}