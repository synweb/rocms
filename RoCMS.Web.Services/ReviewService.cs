using RoCMS.Web.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models;
using RoCMS.Base.Helpers;
using AutoMapper;
using RoCMS.Base.Services;
using RoCMS.Data.Gateways;
using Review = RoCMS.Data.Models.Review;

namespace RoCMS.Web.Services
{
    public class ReviewService : BaseCoreService, IReviewService
    {
        private ReviewGateway _reviewGateway = new ReviewGateway();
        
        protected override int CacheExpirationInMinutes
        {
            get
            {
                return AppSettingsHelper.HoursToExpireCartCache * 60;
            }
        }

        #region IReviewService
        public int CreateReview(Contract.Models.Review review)
        {            
           return _reviewGateway.Insert(Mapper.Map<Review>(review));
        }

        public void DeleteReview(int reviewId)
        {
            _reviewGateway.Delete(reviewId);
        }

        public IEnumerable<Contract.Models.Review> GetModeratedReviewPage(int startIndex, int countOnPage, out int total)
        {
            var rez = _reviewGateway.Select(startIndex, countOnPage, out total, true);
            var list = Mapper.Map<ICollection<Contract.Models.Review>>(rez);
            return list;
        }

        public IList<Contract.Models.Review> GetModeratedReviews(int? count)
        {
            int tmp;
            var rez = _reviewGateway.Select(1, count.HasValue ? count.Value : int.MaxValue , out tmp, true);
            var list = Mapper.Map<ICollection<Contract.Models.Review>>(rez);
            return new List<Contract.Models.Review>(list);
        }

        public Contract.Models.Review GetReview(int id)
        {
            return Mapper.Map<Contract.Models.Review>(_reviewGateway.SelectOne(id));
        }

        public IEnumerable<Contract.Models.Review> GetReviewPage(int startIndex, int countOnPage, out int total)
        {
            var rez = _reviewGateway.Select(startIndex, countOnPage, out total, false);
            var list = Mapper.Map<ICollection<Contract.Models.Review>>(rez);            
            return list;
        }

        public IList<Contract.Models.Review> GetReviews(int? count)
        {
            int tmp;
            var rez = _reviewGateway.Select(1, count.HasValue ? count.Value : int.MaxValue, out tmp, false);            
            var list = Mapper.Map<ICollection<Contract.Models.Review>>(rez);           
            return new List<Contract.Models.Review>(list);
        }

        public void ModerateReview(int reviewId, bool accept)
        {
            var review = GetReview(reviewId);
            review.Moderated = accept;
            _reviewGateway.Update(Mapper.Map<Review>(review));

        }

        public void UpdateReview(Contract.Models.Review review)
        {
            _reviewGateway.Update(Mapper.Map<Review>(review));
        }
        #endregion
    }
}
