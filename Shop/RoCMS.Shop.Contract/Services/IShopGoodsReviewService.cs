using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopGoodsReviewService
    {
        IList<GoodsReview> GetAllGoodsReviews();
        IList<GoodsReview> GetAllGoodsReviewsWithText();
        IList<GoodsReview> GetGoodsReviews(int heartId);
        IList<GoodsReview> GetGoodsReviewsWithText(int heartId);
        IList<GoodsReview> GetGoodsModeratedReviewsWithText(int heartId);
        int CreateGoodsReview(GoodsReview review);
        void UpdateGoodsReview(GoodsReview review);
        void DeleteGoodsReview(int reviewId);
        void AcceptGoodsReview(int reviewId);
        void HideGoodsReview(int reviewId);
        GoodsReview GetGoodsReview(int reviewId);
    }
}
