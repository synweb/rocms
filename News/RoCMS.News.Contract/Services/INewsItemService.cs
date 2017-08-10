﻿using System.Collections.Generic;
using RoCMS.Base.Models;
using RoCMS.Comments.Contract;
using RoCMS.Comments.Contract.Models;
using RoCMS.News.Contract.Models;

namespace RoCMS.News.Contract.Services
{
    public interface INewsItemService: ICommentable
    {
        IEnumerable<NewsItem> GetAllNews();
        ICollection<NewsItem> GetNewsPage(NewsFilter filter, int pageNumber, int pageSize, out int totalCount);
    
        /// <summary>
        /// Добавить новость
        /// </summary>
        /// <param name="news"></param>
        /// <returns>Идентификатор добавленной новости</returns>
        int AddNewsItem(NewsItem news);

        void RemoveNewsItem(int id);

        NewsItem GetNewsItem(int id);

        NewsItem GetNewsItem(string relativeUrl, bool onlyPosted);

        void EditNewsItem(NewsItem news);
        
        bool NewsItemExists(string relativeUrl);

        string GetNewsItemCanonicalUrl(int newsItemId);

        ICollection<int> GetRelatedNewsIds(int newsId, bool withSubnews, int count, bool onlyPosted, int? excludeId);

        ICollection<NewsItem> GetRelatedNews(int newsId, bool withSubnews, int count, bool onlyPosted, int? excludeId);
        int CreateClientPost(NewsItem post);
        void UpdateClientPost(NewsItem post);
        ICollection<NewsItem> GetRandomNews(int count);

        ICollection<RoCMS.News.Contract.Models.TagStat> GetTagStats(int tagCount);

        IEnumerable<string> GetTagByPattern(string pattern, int records);
    }
}
