using System.Collections.Generic;
using RoCMS.News.Contract.Models;

namespace RoCMS.News.Web.Models
{
    public class NewsVM
    {
        public NewsVM(IEnumerable<NewsItem> news, int totalCount, int currentPage, int newsOnPage)
        {
            NewsOnPage = newsOnPage;
            CurrentPage = currentPage;
            TotalCount = totalCount;
            News = news;
        }

        public IEnumerable<NewsItem> News { get; private set; }

        public int TotalCount { get; private set; }

        public int CurrentPage { get; private set; }

        public int NewsOnPage { get; private set; }
    }
}