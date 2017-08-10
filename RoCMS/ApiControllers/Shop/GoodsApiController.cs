﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Shop.Services;
using RoCMS.Web.Contract.Models.Shop;
using RoCMS.Web.Contract.Models.Shop.Exceptions;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers.Shop
{
    [AuthorizeResources(RoCmsResources.Shop)]
    public class GoodsApiController : System.Web.Http.ApiController    {
        private IShopService _shopService;
        private ISettingsService _settingsService;

        public GoodsApiController(IShopService showService, ISettingsService settingsService)
        {
            _shopService = showService;
            _settingsService = settingsService;
        }

        [HttpGet]
        public IList<GoodsItem> GetCategory(int categoryId)
        {
            int total;

            _settingsService.Set<int?>(Web.Contract.Models.SettingKey.LastGoodsCategory, categoryId);
            FilterCollections col;
            return _shopService.GetGoodsSet(new GoodsFilter() {CategoryIds = new[]{categoryId}}, 0, 100, out total, out col, false);
        }

        //[HttpGet]
        //public IList<GoodsItem> SearchByPattern()
        //{
        //    //int total;
        //    //const string pattern = "condtrol измеритель";
        //    //return _shopService.GetGoodsSet(new GoodsFilter() { SearchPattern = pattern }, 1, 100, out total);

        //    //return _shopService.GetGoodsSet(new GoodsFilter() {Articles = new[]{"345"}}, 1, 100, out total);
        //}


        [HttpGet]
        public GoodsItem Get(int goodsId)
        {
            return _shopService.GetGoods(goodsId, false);
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
        public ResultModel Delete(int goodsId)
        {


            try
            {
                _shopService.DeleteGoods(goodsId);
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
            return _shopService.GetAllGoodsReviews();
        }

        [HttpGet]
        public IList<GoodsReview> GetAllGoodsReviewsWithText()
        {
            return _shopService.GetAllGoodsReviewsWithText();          
        }

        [HttpGet]
        public IList<GoodsReview> GetGoodsReviews(int goodsId)
        {
            return _shopService.GetGoodsReviews(goodsId);
        }

        [HttpGet]
        public IList<GoodsReview> GetGoodsReviewsWithText(int goodsId)
        {
            return _shopService.GetGoodsReviewsWithText(goodsId);
            
        }

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
                    IncludeRatingCookie(res, review.GoodsId.ToString());
                }
                else
                {
                    var found = cookie["goodsRated"];
                    var values = found.Value.Split(',').ToList();
                    if (values.Contains(review.GoodsId.ToString()))
                    {
                        //товарищ отправляет рейтинг товара, который он уже оценивал
                        return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    }
                    values.Add(review.GoodsId.ToString());
                    string cookieValue = string.Join(",", values);
                    res = SaveReview(review);
                    IncludeRatingCookie(res, cookieValue);
                }
                return res;
            }
            catch (Exception e)
            {
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
                int id = _shopService.CreateGoodsReview(review);
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
                _shopService.UpdateGoodsReview(review);
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
                _shopService.DeleteGoodsReview(reviewId);
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
                _shopService.AcceptGoodsReview(reviewId);
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
                _shopService.HideGoodsReview(reviewId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
    
}
