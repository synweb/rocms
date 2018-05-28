using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Services
{
    class ShopGoodsReviewService: BaseShopService, IShopGoodsReviewService
    {
        private readonly IHeartService _heartService;


        private readonly GoodsReviewGateway _goodsReviewGateway = new GoodsReviewGateway();

        public ShopGoodsReviewService(IHeartService heartService)
        {
            _heartService = heartService;
        }

        public IList<GoodsReview> GetAllGoodsReviews()
        {
            var dataRes = _goodsReviewGateway.Select();
            var res = Mapper.Map<IList<GoodsReview>>(dataRes);

            foreach (var r in res)
            {
                r.GoodsItem = new GoodsItem() { HeartId = r.HeartId };
                r.GoodsItem.FillHeart(_heartService.GetHeart(r.HeartId));
                    
            }

            return res;
        }

        public IList<GoodsReview> GetAllGoodsReviewsWithText()
        {
            var dataRes = _goodsReviewGateway.Select()
                .Where(x => !string.IsNullOrEmpty(x.Text));
            var res = Mapper.Map<IList<GoodsReview>>(dataRes);
            return res;
        }

        public IList<GoodsReview> GetGoodsReviews(int heartId)
        {
            var dataRes = _goodsReviewGateway.SelectByGoods(heartId);
            var res = Mapper.Map<IList<GoodsReview>>(dataRes);
            return res;
        }

        public double? GetGoodsRating(int heartId)
        {
            var dataRes = _goodsReviewGateway.SelectByGoods(heartId);
            if (dataRes.Any())
            {
                return dataRes.Select(x => x.Rating).Average();
            }
            return null;
        }

        public IList<GoodsReview> GetGoodsReviewsWithText(int heartId)
        {
            var dataRes = _goodsReviewGateway.SelectByGoods(heartId)
                .Where(x => !string.IsNullOrEmpty(x.Text));
            var res = Mapper.Map<IList<GoodsReview>>(dataRes);
            return res;
        }

        public IList<GoodsReview> GetGoodsModeratedReviewsWithText(int heartId)
        {

            var dataRes = _goodsReviewGateway.SelectByGoods(heartId)
                .Where(x => !string.IsNullOrEmpty(x.Text)
                && x.Moderated);
            var res = Mapper.Map<IList<GoodsReview>>(dataRes);
            return res;
        }

        public int CreateGoodsReview(GoodsReview review)
        {
            var dataRec = Mapper.Map<Data.Models.GoodsReview>(review);
            int id = _goodsReviewGateway.Insert(dataRec);
            return id;
        }

        public void UpdateGoodsReview(GoodsReview review)
        {
            var dataRec = Mapper.Map<Data.Models.GoodsReview>(review);
            _goodsReviewGateway.Update(dataRec);
        }

        public void DeleteGoodsReview(int reviewId)
        {
            _goodsReviewGateway.Delete(reviewId);
        }

        public void AcceptGoodsReview(int reviewId)
        {
            var review = _goodsReviewGateway.SelectOne(reviewId);
            review.Moderated = true;
            _goodsReviewGateway.Update(review);
        }

        public void HideGoodsReview(int reviewId)
        {
            var review = _goodsReviewGateway.SelectOne(reviewId);
            review.Moderated = false;
            _goodsReviewGateway.Update(review);
        }

        public GoodsReview GetGoodsReview(int reviewId)
        {
            var dataRes = _goodsReviewGateway.SelectOne(reviewId);
            var res = Mapper.Map<GoodsReview>(dataRes);
            return res;
        }
    }
}
