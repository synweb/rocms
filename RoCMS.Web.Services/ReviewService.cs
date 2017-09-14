using RoCMS.Web.Contract.Services;
using System.Collections.Generic;
using RoCMS.Base.Helpers;
using AutoMapper;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Models;
using Review = RoCMS.Data.Models.Review;

namespace RoCMS.Web.Services
{
    public class ReviewService : BaseCoreService, IReviewService
    {
        private readonly ReviewGateway _reviewGateway = new ReviewGateway();
        private readonly ISettingsService _settingsService;
        private readonly IMailService _mailService;

        public ReviewService(ISettingsService settingsService, IMailService mailService)
        {
            _settingsService = settingsService;
            _mailService = mailService;
        }

        protected override int CacheExpirationInMinutes => AppSettingsHelper.HoursToExpireCartCache * 60;

        #region IReviewService

        public int CreateReview(Contract.Models.Review review, bool notify)
        {
            int id = _reviewGateway.Insert(Mapper.Map<Review>(review));
            if (notify)
            {
                SendReviewNotification(review.Author, review.Email, review.Text);
            }
            return id;
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
            var rez = _reviewGateway.Select(1, count ?? int.MaxValue, out tmp, false);            
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

        private void SendReviewNotification(string author, string authorEmail, string text)
        {   
            string template = _settingsService.GetSettings<string>("MailTmplReviewCreated");
            string body = string.Format(template, author, authorEmail, text);
            MailMsg reply = new MailMsg
            {
                Subject = "Кто-то оставил отзыв на сайте",
                Receiver = _settingsService.GetSettings<string>(nameof(Setting.OrderEmailAddress)),
                Body = body
            };
            _mailService.Send(reply);
        }
        #endregion
    }
}
