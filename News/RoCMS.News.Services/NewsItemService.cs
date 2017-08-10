using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Base.Extentions;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.Comments.Contract.Models;
using RoCMS.Comments.Contract.Services;
using RoCMS.Comments.Contract.ViewModels;
using RoCMS.News.Contract.Models;
using RoCMS.News.Contract.Services;
using RoCMS.News.Data;
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
        private readonly ICommentService _commentService;
        private readonly CategoryGateway _categoryGateway = new CategoryGateway();
        private readonly NewsItemCategoryGateway _newsItemCategoryGateway = new NewsItemCategoryGateway();
        private readonly NewsItemGateway _newsItemGateway = new NewsItemGateway();
        private readonly TagGateway _tagGateway = new TagGateway();
        private readonly NewsItemTagGateway _newsItemTagGateway = new NewsItemTagGateway();

        private readonly INewsCategoryService _categoryService;
        private readonly IBlogService _blogService;

        public NewsItemService(ICommentService commentService, ISearchService searchService, INewsCategoryService categoryService, IBlogService blogService)
        {
            _commentService = commentService;
            _searchService = searchService;
            _categoryService = categoryService;
            _blogService = blogService;

            InitCache("TagMemoryCache");
            CacheExpirationInMinutes = 30;
        }


        //Для генерации урлов в уже существующих новостях
        //private void GenerateRelativeUrls()
        //{
        //    using (var context = Context)
        //    {
        //        foreach (var item in context.NewsItems)
        //        {
        //            if (String.IsNullOrEmpty(item.RelativeUrl))
        //            {
        //                item.RelativeUrl = TranslitHelper.ToTranslitedUrl(item.Title);
        //            }
        //        }

        //        context.SaveChanges();
        //    }
        //}

        public int CreateComment(int targetId, Comment comment)
        {
            var item = _newsItemGateway.SelectOne(targetId);
            using (var ts = new TransactionScope())
            {
                if (item.CommentTopicId == null)
                {
                    var commentTopic = new CommentTopic()
                    {
                        TargetId = item.NewsId,
                        TargetUrl = "/News/" + targetId,
                        TargetTitle = item.Title,
                        TargetType = "News"
                    };
                    int commentTopicId = _commentService.CreateTopic(commentTopic);
                    item.CommentTopicId = commentTopicId;
                    _newsItemGateway.Update(item);
                }
                comment.CommentTopicId = item.CommentTopicId.Value;
                int res = _commentService.CreateComment(comment);

                ts.Complete();
                return res;
            }
        }

        public void DeleteComment(int commentId)
        {
            _commentService.DeleteComment(commentId);
        }

        public void UpdateCommentText(int commentId, string text)
        {
            _commentService.UpdateCommentText(commentId, text);
        }

        public void ModerateComment(int commentId, bool moderated)
        {
            _commentService.ModerateComment(commentId, moderated);
        }

        public ICollection<CommentVM> GetThread(int targetId)
        {
            var res = _commentService.GetThreadsByTopic(targetId, false);
            return res;
        }

        public Comment GetComment(int commentId)
        {
            var res = _commentService.GetComment(commentId);
            return res;
        }

        public IEnumerable<NewsItem> GetAllNews()
        {
            var dataRes = _newsItemGateway.Select();
            var res = Mapper.Map<ICollection<NewsItem>>(dataRes);
            foreach (var newsItem in res)
            {
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

            foreach (var newsItem in res)
            {
                FillItem(newsItem);
            }
            return res;
        }

        public int AddNewsItem(NewsItem news)
        {
            var dataRec = Mapper.Map<Data.Models.NewsItem>(news);
            using (var ts = new TransactionScope())
            {
                bool relativeUrlExists = _newsItemGateway.SelectByUrl(news.RelativeUrl, false) != null;
                int relativeUrlCounter = 2;
                while (relativeUrlExists)
                {
                    string newRelativeUrl = $"{news.RelativeUrl}-{relativeUrlCounter}";
                    relativeUrlCounter++;
                    relativeUrlExists = _newsItemGateway.SelectByUrl(newRelativeUrl, false) != null;
                    if (!relativeUrlExists)
                    {
                        news.RelativeUrl = newRelativeUrl;
                    }
                }

                news.NewsId = _newsItemGateway.Insert(dataRec);
                var tags = news.Tags.Split(',').Select(x => x.Trim().ToLower());

                var existingTags = _tagGateway.Select();
                var existingTagNames = existingTags.Select(x => x.Name).ToArray();
                foreach (var tag in tags.Except(existingTagNames))
                {
                    int tagId = _tagGateway.Insert(tag);
                    _newsItemTagGateway.Insert(news.NewsId, tagId);
                }
                foreach (var tag in existingTags.Where(x => tags.Contains(x.Name)))
                {
                    _newsItemTagGateway.Insert(news.NewsId, tag.TagId);
                }

                var catIds = news.Categories.Select(x => x.ID);
                foreach (var catId in catIds)
                {
                    _newsItemCategoryGateway.Insert(news.NewsId, catId);
                }
                _searchService.UpdateIndex(news);
                ts.Complete();
                return news.NewsId;
            }
        }

        public void RemoveNewsItem(int id)
        {
            using (var ts = new TransactionScope())
            {
                _newsItemGateway.Delete(id);
                _tagGateway.DeleteUnassociated();
                _searchService.RemoveFromIndex(typeof(NewsItem), id);
                ts.Complete();
            }
        }

        public NewsItem GetNewsItem(int id)
        {
            var dataRes = _newsItemGateway.SelectOne(id);
            var res = Mapper.Map<NewsItem>(dataRes);
            FillItem(res);
            return res;
        }

        public NewsItem GetNewsItem(string relativeUrl, bool onlyPosted)
        {
            var dataRes = _newsItemGateway.SelectByUrl(relativeUrl, true);
            if (dataRes == null)
                return null;
            var res = Mapper.Map<NewsItem>(dataRes);
            FillItem(res);
            return res;
        }

        public void EditNewsItem(NewsItem news)
        {
            //функциональность чревата дублями, если не обновлять страницу после сохранения
            //var originNewsItem = _newsItemGateway.SelectOne(news.NewsId);
            //if (news.RelativeUrl != originNewsItem.RelativeUrl)
            //{
            //    bool relativeUrlExists = _newsItemGateway.SelectByUrl(news.RelativeUrl) != null;
            //    int relativeUrlCounter = 2;
            //    while (relativeUrlExists)
            //    {
            //        string newRelativeUrl = $"{news.RelativeUrl}-{relativeUrlCounter}";
            //        relativeUrlCounter++;
            //        relativeUrlExists = _newsItemGateway.SelectByUrl(newRelativeUrl) != null;
            //        if (!relativeUrlExists)
            //        {
            //            news.RelativeUrl = newRelativeUrl;
            //        }
            //    }
            //}

            var originNewsItem = _newsItemGateway.SelectOne(news.NewsId);
            if (news.RelativeUrl != originNewsItem.RelativeUrl)
            {
                bool relativeUrlExists = _newsItemGateway.SelectByUrl(news.RelativeUrl, false) != null;
                if (relativeUrlExists)
                {
                    throw new ArgumentException("Запись с таким адресом уже существует");
                }
            }

            var dataRec = Mapper.Map<Data.Models.NewsItem>(news);
            var tags = !string.IsNullOrEmpty(news.Tags) ? news.Tags.Split(',').Select(x => x.Trim().ToLower()).ToList() : new List<string>();
            var originalTags = _tagGateway.SelectByNews(news.NewsId);
            var existingTags = _tagGateway.Select();
            var existingTagNames = existingTags.Select(x => x.Name).ToArray();

            var existingCatIds = _newsItemCategoryGateway.SelectCategoryIdsByNews(news.NewsId);
            var catIds = news.Categories.Select(x => x.ID).ToArray();

            using (var ts = new TransactionScope())
            {
                _newsItemGateway.Update(dataRec);

                foreach (var tag in tags.Except(existingTagNames))
                {
                    int tagId = _tagGateway.Insert(tag);
                    _newsItemTagGateway.Insert(news.NewsId, tagId);
                }
                foreach (var tag in existingTags.Where(x => tags.Contains(x.Name) && originalTags.All(y => y.TagId != x.TagId)))
                {
                    _newsItemTagGateway.Insert(news.NewsId, tag.TagId);
                }

                if (originalTags.Any(x => !tags.Contains(x.Name)))
                {
                    foreach (var tag in originalTags.Where(x => !tags.Contains(x.Name)))
                    {
                        _newsItemTagGateway.Delete(news.NewsId, tag.TagId);
                    }
                    _tagGateway.DeleteUnassociated();
                }

                foreach (var catId in catIds.Except(existingCatIds))
                {
                    _newsItemCategoryGateway.Insert(news.NewsId, catId);
                }
                foreach (var catId in existingCatIds.Except(catIds))
                {
                    _newsItemCategoryGateway.Delete(news.NewsId, catId);
                }
                _searchService.UpdateIndex(news);
                ts.Complete();
            }
        }
        
        private void FillItem(NewsItem item)
        {
            // tags
            FillTags(item);

            // cats
            var catsIds = _newsItemCategoryGateway.SelectCategoryIdsByNews(item.NewsId);
            var cats = catsIds.Select(x => _categoryGateway.SelectOne(x));
            foreach (var category in cats)
            {
                item.Categories.Add(new IdNamePair<int>(category.CategoryId, category.Name));
            }
            item.CanonicalUrl = GetNewsItemCanonicalUrl(item.NewsId);
        }

        public bool NewsItemExists(string relativeUrl)
        {
            return _newsItemGateway.SelectByUrl(relativeUrl, false) != null;
        }

        public string GetNewsItemCanonicalUrl(int newsItemId)
        {
            var newsItem = _newsItemGateway.SelectOne(newsItemId);
            var catsIds = _newsItemCategoryGateway.SelectCategoryIdsByNews(newsItemId);
            if (catsIds.Any())
            {
                return $"{_categoryService.GetCategoryCannonicalUrl(catsIds.First())}/{newsItem.RelativeUrl}";
            }
            else
            {
                return newsItem.RelativeUrl;
            }
        }

        public int CreateClientPost(NewsItem post)
        {
            if(string.IsNullOrEmpty(post.Title)
                || string.IsNullOrEmpty(post.Text)
                || string.IsNullOrEmpty(post.Description))
            {
                throw new ArgumentException();
            }
            if (!_blogService.CheckIfUserHasAccess(post.AuthorId, post.BlogId))
            {
                throw new UnauthorizedAccessException();
            }
            post.CreationDate = post.PostingDate = DateTime.UtcNow;
            post.Text = FormattingHelper.AddRelNofollowToAnchors(post.Text);
            post.RecordType = RecordType.Default;
            post.RelativeUrl = TranslitHelper.ToTranslitedUrl(post.Title);
            
            var dataRec = Mapper.Map<Data.Models.NewsItem>(post);
            int id = _newsItemGateway.Insert(dataRec);
            post.NewsId = id;
            _searchService.UpdateIndex(post);
            return id;
        }

        public void UpdateClientPost(NewsItem post)
        {
            if (string.IsNullOrEmpty(post.Title)
                || string.IsNullOrEmpty(post.Text)
                || string.IsNullOrEmpty(post.Description))
            {
                throw new ArgumentException();
            }
            post.Text = FormattingHelper.AddRelNofollowToAnchors(post.Text);
            post.RelativeUrl = TranslitHelper.ToTranslitedUrl(post.Title);
            var dataRec = Mapper.Map<Data.Models.NewsItem>(post);
            _newsItemGateway.Update(dataRec);
            _searchService.UpdateIndex(post);
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
            var tags = _tagGateway.SelectByNews(item.NewsId).Select(x => x.Name);
            item.Tags = string.Join(",", tags);
        }

        public ICollection<RoCMS.News.Contract.Models.TagStat> GetTagStats(int tagCount)
        {
            return Mapper.Map<ICollection<RoCMS.News.Contract.Models.TagStat>>(_newsItemTagGateway.SelectTagStats(tagCount));
        }

        public IEnumerable<string> GetTagByPattern(string pattern, int records)
        {
            if (pattern.Length < TagCacheLenght)
            {
                return new List<string>();
            }           

            var rez = GetFromCacheOrLoadAndAddToCache<IEnumerable<string>>(pattern, () =>
            {
                return _tagGateway.SelectByPattern(pattern, records);
            });

            return rez.Where(x => x.StartsWith(pattern));
        }

        protected override int CacheExpirationInMinutes { get; }
    }
}
