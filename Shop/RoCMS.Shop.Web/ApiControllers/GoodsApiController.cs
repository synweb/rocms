using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Models.Exceptions;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    [AuthorizeResourcesApi(ShopRoCmsResources.Shop)]
    public class GoodsApiController : System.Web.Http.ApiController    
    {
        private readonly IShopService _shopService;
        private readonly ISettingsService _settingsService;
        private readonly IShopGoodsReviewService _shopGoodsReviewService;
        private readonly ILogService _logService;

        public GoodsApiController(IShopService shopService, ISettingsService settingsService, IShopGoodsReviewService shopGoodsReviewService, ILogService logService)
        {
            _shopService = shopService;
            _settingsService = settingsService;
            _shopGoodsReviewService = shopGoodsReviewService;
            _logService = logService;
        }

        [HttpGet]
        public IList<GoodsItem> GetCategory(int categoryId)
        {
            int total;

            _settingsService.Set<int?>(SettingKey.LastGoodsCategory.ToString(), categoryId);
            FilterCollections col;
            return _shopService.GetGoodsSet(new GoodsFilter() {CategoryIds = new[]{new []{categoryId}}}, 0, int.MaxValue, out total, out col, false);
        }

        //[HttpGet]
        //public IList<GoodsItem> SearchByPattern()
        //{
        //    //int total;
        //    //const string pattern = "condtrol измеритель";
        //    //return _shopService.GetGoodsSet(new GoodsFilter() { SearchPattern = pattern }, 1, 100, out total);

        //    //return _shopService.GetGoodsSet(new GoodsFilter() {Articles = new[]{"345"}}, 1, 100, out total);
        //}

        [HttpPost]
        public IList<GoodsItem> GetGoods(ExtendedGoodsFilter filter)
        {
            int total;
            FilterCollections col;

            _settingsService.Set<int?>(SettingKey.LastGoodsCategory.ToString(), 
                filter.CategoryIds != null 
                && filter.CategoryIds.Any()
                && filter.CategoryIds.SelectMany(x => x).Any()
                ? filter.CategoryIds.First().First() : (int?)null);
            _settingsService.Set<int?>(SettingKey.LastGoodsSupplier.ToString(), filter.SupplierIds != null && filter.SupplierIds.Any() ? filter.SupplierIds.First() : (int?)null);
            _settingsService.Set<int?>(SettingKey.LastGoodsManufacturer.ToString(), filter.ManufacturerIds != null && filter.ManufacturerIds.Any() ? filter.ManufacturerIds.First() : (int?)null);
            _settingsService.Set<SortCriterion>(SettingKey.LastGoodsSortBy.ToString(), filter.SortBy);

            var res = _shopService.GetGoodsSet(filter, filter.StartIndex, filter.Count, out total, out col, false);
            return res;
        }

        public class ExtendedGoodsFilter : GoodsFilter
        {
            public int Count { get; set; }
            public int StartIndex { get; set; }
        }

        [HttpGet]
        public GoodsItem Get(int heartId)
        {
            return _shopService.GetGoods(heartId, false);
        }

        [HttpPost]
        public ResultModel Create(GoodsItem goods)
        {
            try
            {
                int id = _shopService.CreateGoods(goods);
                var res = new ResultModel(true, new { id = id });
                return res;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Update(GoodsItem goods)
        {
            try
            {
                _shopService.UpdateGoods(goods);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel Delete(int heartId)
        {
            try
            {
                _shopService.DeleteGoods(heartId);
                return ResultModel.Success;
            }
            catch (GoodsNotFoundException)
            {
                return new ResultModel(false, "Товар отсутствует в базе");
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }


        [HttpGet]
        public IList<GoodsReview> GetAllGoodsReviews()
        {
            return _shopGoodsReviewService.GetAllGoodsReviews();
        }

        [HttpGet]
        public IList<GoodsReview> GetAllGoodsReviewsWithText()
        {
            return _shopGoodsReviewService.GetAllGoodsReviewsWithText();          
        }

        [HttpGet]
        public IList<GoodsReview> GetGoodsReviews(int heartId)
        {
            return _shopGoodsReviewService.GetGoodsReviews(heartId);
        }

        [HttpGet]
        public IList<GoodsReview> GetGoodsReviewsWithText(int heartId)
        {
            return _shopGoodsReviewService.GetGoodsReviewsWithText(heartId);
            
        }

        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage CreateGoodsReview(GoodsReview review)
        {
            try
            {
                if (review.Author == "")
                {
                    review.Author = null;
                }
                if (review.AuthorContact == "")
                {
                    review.AuthorContact = null;
                }
                if (review.Text == "")
                {
                    review.Text = null;
                }
                if (review.Rating < 1 || review.Rating > 5)
                {
                    review.Rating = null;
                }

                HttpResponseMessage res;
                if (review.Rating == null)
                {
                    return SaveReview(review);
                }


                CookieHeaderValue cookie = Request.Headers.GetCookies("goodsRated").FirstOrDefault();
                if (cookie == null)
                {
                    res = SaveReview(review);
                    IncludeRatingCookie(res, review.HeartId.ToString());
                }
                else
                {
                    var found = cookie["goodsRated"];
                    var values = found.Value.Split(',').ToList();
                    if (values.Contains(review.HeartId.ToString()))
                    {
                        //товарищ отправляет рейтинг товара, который он уже оценивал
                        return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    }
                    values.Add(review.HeartId.ToString());
                    string cookieValue = string.Join(",", values);
                    res = SaveReview(review);
                    IncludeRatingCookie(res, cookieValue);
                }
                return res;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Сохранить отзыв в БД и вернуть результат с ID отзыва
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        private HttpResponseMessage SaveReview(GoodsReview review)
        {
            int id = _shopGoodsReviewService.CreateGoodsReview(review);
                return new HttpResponseMessage();
        }

        /// <summary>
        /// Включает в отправляемый результат куку с только что оценённым товаром
        /// </summary>
        /// <param name="result"></param>
        /// <param name="cookieValue"></param>
        private void IncludeRatingCookie(HttpResponseMessage result, string cookieValue)
        {
            var cookie = new CookieHeaderValue("goodsRated", cookieValue);
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";
            result.Headers.AddCookies(new CookieHeaderValue[] { cookie });
        }

        [HttpPost]
        public ResultModel UpdateGoodsReview(GoodsReview review)
        {
            try
            {
                _shopGoodsReviewService.UpdateGoodsReview(review);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
        
        [HttpPost]
        public ResultModel DeleteGoodsReview(int reviewId)
        {
            try
            {
                _shopGoodsReviewService.DeleteGoodsReview(reviewId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
        [HttpPost]
        public ResultModel AcceptReview(int reviewId)
        {
            try
            {
                _shopGoodsReviewService.AcceptGoodsReview(reviewId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel HideReview(int reviewId)
        {
            try
            {
                _shopGoodsReviewService.HideGoodsReview(reviewId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
    
}
