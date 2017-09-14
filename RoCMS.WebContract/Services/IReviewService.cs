using System.Collections.Generic;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IReviewService
    {
        int CreateReview(Review review, bool notify);
        Review GetReview(int id);
        void UpdateReview(Review review);
        void ModerateReview(int reviewId, bool accept);
        void DeleteReview(int reviewId);
        IList<Review> GetReviews(int? count);
        IList<Review> GetModeratedReviews(int? count);
        IEnumerable<Review> GetReviewPage(int startIndex, int countOnPage, out int total);
        IEnumerable<Review> GetModeratedReviewPage(int startIndex, int countOnPage, out int total);
    }
}
