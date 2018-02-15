using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AutoMapper;
using RoCMS.Base.Exceptions;
using RoCMS.Base.Extentions;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.News.Contract.Services;
using RoCMS.News.Data.Gateways;
using RoCMS.News.Data.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Search;
using RoCMS.Web.Contract.Services;
using NewsFilter = RoCMS.News.Contract.Models.NewsFilter;
using NewsItem = RoCMS.News.Contract.Models.NewsItem;
using RecordType = RoCMS.News.Contract.Models.RecordType;

namespace RoCMS.News.Services
{
    public class NewsItemService : NewsService, INewsItemService
    {
        private const int TagCacheLenght = 2; // количество букв по которым ищутся тэги
        private readonly ISearchService _searchService;

        private readonly CategoryGateway _categoryGateway = new CategoryGateway();
        private readonly NewsItemCategoryGateway _newsItemCategoryGateway = new NewsItemCategoryGateway();
        private readonly NewsItemGateway _newsItemGateway = new NewsItemGateway();
        private readonly TagGateway _tagGateway = new TagGateway();
        private readonly NewsItemTagGateway _newsItemTagGateway = new NewsItemTagGateway();
        
        private readonly INewsCategoryService _categoryService;
        private readonly IHeartService _heartService;
        private readonly IBlogService _blogService;

        public NewsItemService(ISearchService searchService, INewsCategoryService categoryService, IBlogService blogService, IHeartService heartService)
        {

            _searchService = searchService;
            _categoryService = categoryService;
            _blogService = blogService;
            _heartService = heartService;

            InitCache("TagMemoryCache");
            CacheExpirationInMinutes = 30;
        }

        public IEnumerable<NewsItem> GetAllNews()
        {
            var dataRes = _newsItemGateway.Select();
            var res = Mapper.Map<ICollection<NewsItem>>(dataRes);

            var hearts = _heartService.GetHearts(res.Select(x => x.HeartId));

            foreach (var newsItem in res)
            {
                var heart = hearts.Single(x => x.HeartId == newsItem.HeartId);
                newsItem.FillHeart(heart);
                FillItem(newsItem);
            }
            return res;
        }
        
        private ICollection<int> FilterByCategories(ICollection<int> newsIds, string categoryQuery)
        {
            if (string.IsNullOrEmpty(categoryQuery))
            {
                return newsIds;
            }
            int[][] catSets = null;
            // получение коллекций категорий, по которым будем фильтровать айдишники
            // cats format: 1,2,3;5,6;9 => (1|2|3) & (5|6) & 9
            catSets = categoryQuery.Split(';').Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();
            IEnumerable<int> filteredNewsIds = newsIds;
            if (catSets.Any())
            {
                foreach (var catSet in catSets)
                {
                    var catsNews = catSet.SelectMany(x => _newsItemCategoryGateway.SelectNewsIdsByCategory(x));
                    filteredNewsIds = filteredNewsIds.Intersect(catsNews);
                }
                if (!filteredNewsIds.Any()) return new List<int>();
            }
            return filteredNewsIds.ToList();
        }

        private ICollection<int> FilterByTag(ICollection<int> newsIds, string tagName)
        {
            var ids = _newsItemGateway.SelectIdsByTagName(tagName, false);
            return ids.Intersect(newsIds).ToList();
        }

        public ICollection<NewsItem> GetNewsPage(NewsFilter filter, int pageNumber, int pageSize, out int totalCount)
        {
            ICollection<int> newsIds;
            if (string.IsNullOrWhiteSpace(filter.TextQuery))
            {
                // если поискового запроса нет, забираем айдишники вообще всех новостей
                //TODO: для ускорения можно закэшировать
                newsIds = _newsItemGateway.SelectIds();
            }
            else
            {
                // если есть поисковый запрос, забираем из поискового индекса айдишники нужных
                int searchTotal;
                var searchResults =
                    _searchService.Search(new SearchParams(filter.TextQuery, new Type[] {typeof(NewsItem)}),
                        out searchTotal, 1, int.MaxValue);
                newsIds = searchResults.Select(x => int.Parse(x.EntityId)).Distinct().ToList();
            }
            if (!newsIds.Any())
            {
                // если после фильтрации ничего не осталось, возвращаем пустой список
                totalCount = 0;
                return new List<NewsItem>();
            }

            if (!string.IsNullOrEmpty(filter.CategoryQuery))
            {
                newsIds = FilterByCategories(newsIds, filter.CategoryQuery);
                if (!newsIds.Any())
                {
                    // если после фильтрации ничего не осталось, возвращаем пустой список
                    totalCount = 0;
                    return new List<NewsItem>();
                }
            }

            if (!string.IsNullOrEmpty(filter.TagName))
            {
                newsIds = FilterByTag(newsIds, filter.TagName);
                if (!newsIds.Any())
                {
                    // если после фильтрации ничего не осталось, возвращаем пустой список
                    totalCount = 0;
                    return new List<NewsItem>();
                }
            }

            var dataFilter = new FinalNewsFilter(){
                OnlyPosted = filter.OnlyPosted,
                NewsIds = newsIds,
                BlogId = filter.BlogId,
                OnlyFutureEventDate = filter.OnlyFutureEventDate,
                RecordTypes = filter.RecordTypes.Select(x => x.ToString()).ToArray(),
                SortBy = filter.SortBy.ToString(),
                SortOrder =  filter.SortOrder
            };
            var dataRes = _newsItemGateway.SelectFilteredPage(dataFilter, pageNumber, pageSize, out totalCount);
            var res = Mapper.Map<ICollection<NewsItem>>(dataRes);

            var hearts = _heartService.GetHearts(res.Select(x => x.HeartId));

            foreach (var newsItem in res)
            {
                var heart = hearts.Single(x => x.HeartId == newsItem.HeartId);
                newsItem.FillHeart(heart);
                newsItem.CanonicalUrl = _heartService.GetCanonicalUrl(heart.HeartId);
                FillItem(newsItem);
            }
            return res;
        }

        private static object _createNewsItemLockObj = new object();

        public int CreateNewsItem(NewsItem news)
        {
            news.Type = news.GetType().FullName;
            var dataRec = Mapper.Map<Data.Models.NewsItem>(news);
            lock (_createNewsItemLockObj)
            {
                using (var ts = new TransactionScope())
                {
                    news.RelativeUrl = _heartService.GetNextAvailableRelativeUrl(news.RelativeUrl);
                    news.HeartId = dataRec.HeartId = _heartService.CreateHeart(news);
                    _newsItemGateway.Insert(dataRec);
                    var tags = news.Tags?.Split(',').Select(x => x.Trim().ToLower()).ToArray() ?? new string[0];

                    var existingTags = _tagGateway.Select();
                    var existingTagNames = existingTags.Select(x => x.Name).ToArray();
                    foreach (var tag in tags.Except(existingTagNames))
                    {
                        int tagId = _tagGateway.Insert(tag);
                        _newsItemTagGateway.Insert(news.HeartId, tagId);
                    }
                    foreach (var tag in existingTags.Where(x => tags.Contains(x.Name)))
                    {
                        _newsItemTagGateway.Insert(news.HeartId, tag.TagId);
                    }

                    var catIds = news.Categories.Select(x => x.ID);
                    foreach (var catId in catIds)
                    {
                        _newsItemCategoryGateway.Insert(news.HeartId, catId);
                    }
                    _searchService.UpdateIndex(news);
                    ts.Complete();
                    return news.HeartId;
                }
            }
        }

        public void RemoveNewsItem(int id)
        {
            using (var ts = new TransactionScope())
            {
                //var heart = _heartService.GetHeart(id);
                _heartService.DeleteHeart(id);
                _tagGateway.DeleteUnassociated();
                _searchService.RemoveFromIndex(typeof(NewsItem), id);
                ts.Complete();

            }
        }

        public NewsItem GetNewsItem(int id)
        {
            var heart = _heartService.GetHeart(id);
            if (heart == null)
                throw new UrlNotFoundException();
            var dataRes = _newsItemGateway.SelectOne(heart.HeartId);
            var res = Mapper.Map<NewsItem>(dataRes);
            res.FillHeart(heart);
            FillItem(res);
            return res;
        }

        public NewsItem GetNewsItem(string relativeUrl, bool onlyPosted)
        {
            var heart = _heartService.GetHeart(relativeUrl);
            if (heart == null)
                throw new UrlNotFoundException(relativeUrl);
            var dataRes = _newsItemGateway.SelectOne(heart.HeartId);
            var res = Mapper.Map<NewsItem>(dataRes);
            res.FillHeart(heart);
            FillItem(res);
            return res;
        }

        public void UpdateNewsItem(NewsItem news)
        {
            var originHeart = _heartService.GetHeart(news.HeartId);
            if (news.RelativeUrl != originHeart.RelativeUrl)
            {

                bool relativeUrlExists = _heartService.CheckIfUrlExists(news.RelativeUrl);
                if (relativeUrlExists)
                {
                    throw new ArgumentException("Этот адрес уже занят");
                }
            }

            var dataRec = Mapper.Map<Data.Models.NewsItem>(news);
            var tags = !string.IsNullOrEmpty(news.Tags) ? news.Tags.Split(',').Select(x => x.Trim().ToLower()).ToList() : new List<string>();
            var originalTags = _tagGateway.SelectByNews(news.HeartId);
            var existingTags = _tagGateway.Select();
            var existingTagNames = existingTags.Select(x => x.Name).ToArray();

            var existingCatIds = _newsItemCategoryGateway.SelectCategoryIdsByNews(news.HeartId);
            var catIds = news.Categories.Select(x => x.ID).ToArray();

            using (var ts = new TransactionScope())
            {

                _heartService.UpdateHeart(news);

                _newsItemGateway.Update(dataRec);

                foreach (var tag in tags.Except(existingTagNames))
                {
                    int tagId = _tagGateway.Insert(tag);
                    _newsItemTagGateway.Insert(news.HeartId, tagId);
                }
                foreach (var tag in existingTags.Where(x => tags.Contains(x.Name) && originalTags.All(y => y.TagId != x.TagId)))
                {
                    _newsItemTagGateway.Insert(news.HeartId, tag.TagId);
                }

                if (originalTags.Any(x => !tags.Contains(x.Name)))
                {
                    foreach (var tag in originalTags.Where(x => !tags.Contains(x.Name)))
                    {
                        _newsItemTagGateway.Delete(news.HeartId, tag.TagId);
                    }
                    _tagGateway.DeleteUnassociated();
                }

                foreach (var catId in catIds.Except(existingCatIds))
                {
                    _newsItemCategoryGateway.Insert(news.HeartId, catId);
                }
                foreach (var catId in existingCatIds.Except(catIds))
                {
                    _newsItemCategoryGateway.Delete(news.HeartId, catId);
                }
                _searchService.UpdateIndex(news);
                ts.Complete();
            }
        }

        public bool NewsItemExists(int id)
        {
            // TODO: можно ускорить отдельной хранимкой
            return _newsItemGateway.SelectOne(id) != null;
        }

        private void FillItem(NewsItem item)
        {
            // tags
            FillTags(item);

            // cats
            var catsIds = _newsItemCategoryGateway.SelectCategoryIdsByNews(item.HeartId);
            var cats = catsIds.Select(x => _categoryGateway.SelectOne(x));
            foreach (var category in cats)
            {
                item.Categories.Add(new IdNamePair<int>(category.CategoryId, category.Name));
            }
        }

        public bool NewsItemExists(string relativeUrl)
        {
            return _heartService.CheckIfUrlExists(relativeUrl);
        }

        public int CreateClientPost(NewsItem post)
        {
            throw new NotImplementedException();
            //if(string.IsNullOrEmpty(post.Title)
            //    || string.IsNullOrEmpty(post.Text)
            //    || string.IsNullOrEmpty(post.Description))
            //{
            //    throw new ArgumentException();
            //}
            //if (!_blogService.CheckIfUserHasAccess(post.AuthorId, post.BlogId))
            //{
            //    throw new UnauthorizedAccessException();
            //}
            //post.CreationDate = post.PostingDate = DateTime.UtcNow;
            //post.Text = FormattingHelper.AddRelNofollowToAnchors(post.Text);
            //post.RecordType = RecordType.Default;
            //post.RelativeUrl = TranslitHelper.ToTranslitedUrl(post.Title);
            
            //var dataRec = Mapper.Map<Data.Models.NewsItem>(post);
            //int id = _newsItemGateway.Insert(dataRec);
            //post.HeartId = id;
            //_searchService.UpdateIndex(post);
            //return id;
        }

        public void UpdateClientPost(NewsItem post)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrEmpty(post.Title)
            //    || string.IsNullOrEmpty(post.Text)
            //    || string.IsNullOrEmpty(post.Description))
            //{
            //    throw new ArgumentException();
            //}
            //post.Text = FormattingHelper.AddRelNofollowToAnchors(post.Text);
            //post.RelativeUrl = TranslitHelper.ToTranslitedUrl(post.Title);
            //var dataRec = Mapper.Map<Data.Models.NewsItem>(post);
            //_newsItemGateway.Update(dataRec);
            //_searchService.UpdateIndex(post);
        }

        /// <summary>
        /// Вытаскивает связанные новости, при этом newsId включается в результирующий массив.
        /// Переданный в excludeId исключается из результата. 
        /// Нужно для получения связанных новостей по RelativeNewsItemId новости, а не по ее NewsId
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="withSubnews"></param>
        /// <param name="count"></param>
        /// <param name="onlyPosted"></param>
        /// <param name="excludeId"></param>
        /// <returns></returns>
        public ICollection<int> GetRelatedNewsIds(int newsId, bool withSubnews, int count, bool onlyPosted, int? excludeId)
        {
            var res = _newsItemGateway.SelectRelated(newsId, withSubnews, count, onlyPosted).ToList();
            res.Add(newsId);
            if (excludeId.HasValue)
            {
                res.Remove(excludeId.Value);
            }
            return res;
        }

        public ICollection<NewsItem> GetRelatedNews(int newsId, bool withSubnews, int count, bool onlyPosted, int? excludeId)
        {
            var ids = GetRelatedNewsIds(newsId, withSubnews, count, onlyPosted, excludeId);
            var result = new List<NewsItem>();
            foreach (var id in ids)
            {
                var news = GetNewsItem(id);
                result.Add(news);
            }
            return result;
        }

        public ICollection<NewsItem> GetRandomNews(int count)
        {
            if (count < 0)
                throw new ArgumentException("Count is less than zero", nameof(count));
            var res = new List<NewsItem>(count);
            if (count == 0)
                return res;
            var ids = _newsItemGateway.SelectIds(true);
            if (!ids.Any())
                return res;

            var rndIds = ids.TakeRandom(count);
            foreach (var rndId in rndIds)
            {
                var dataRec = _newsItemGateway.SelectOne(rndId);
                var rec = Mapper.Map<NewsItem>(dataRec);
                FillTags(rec);
                res.Add(rec);
            }
            return res;
        }

        private void FillTags(NewsItem item)
        {
            var tags = _tagGateway.SelectByNews(item.HeartId).Select(x => x.Name);
            item.Tags = string.Join(",", tags);
        }

        public ICollection<Contract.Models.TagStat> GetTagStats(int tagCount)
        {
            return Mapper.Map<ICollection<Contract.Models.TagStat>>(_newsItemTagGateway.SelectTagStats(tagCount));
        }

        public IEnumerable<string> GetTagByPattern(string pattern, int records)
        {
            if (pattern.Length < TagCacheLenght)
            {
                return new List<string>();
            }           

            var rez = GetFromCacheOrLoadAndAddToCache<IEnumerable<string>>(pattern, () => _tagGateway.SelectByPattern(pattern, records));

            return rez.Where(x => x.StartsWith(pattern));
        }

        public void IncreaseViewCount(int newsId)
        {
            _newsItemGateway.IncreaseViewCount(newsId);
        }

        protected override int CacheExpirationInMinutes { get; }
    }
}
