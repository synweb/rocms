using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using AutoMapper;
using RoCMS.Base.Services;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Models.Search;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class SearchService : BaseCoreService, ISearchService
    {

        protected override int CacheExpirationInMinutes
        {
            get { return 30; }
        }

        private readonly SearchItemGateway _searchItemGateway = new SearchItemGateway();
        
        public SearchService()
        {
            _rules = new Dictionary<Type, ICollection<IndexingRule>>();
            InitCache();
        }


        public void RegisterRules(Type type, ICollection<IndexingRule> rules)
        {
            if (_rules.ContainsKey(type))
            {
                _rules[type] = rules;
            }
            else
            {
                _rules.Add(type, rules);
            }
        }

        public void UpdateIndex(ISearchable item)
        {
            if (!_rules.ContainsKey(item.GetType()))
            {
                throw new UnknownRuleException();
            }
            var rules = _rules[item.GetType()];
            foreach (var rule in rules)
            {
                var searchItem = rule(item);
                if (string.IsNullOrEmpty(searchItem.SearchContent))
                {
                    continue;
                }
                var dataItem = Mapper.Map<Data.Models.SearchItem>(searchItem);
                _searchItemGateway.Upsert(dataItem);
            }
            ClearCache();
        }

        public void RemoveFromIndex(Type type, object id)
        {
            string stringId = id.ToString();
            _searchItemGateway.Delete(type.FullName, stringId);
            ClearCache();
        }

        private IDictionary<Type, ICollection<IndexingRule>> _rules;

        private class SearchResultEqualityComparer: IEqualityComparer<Data.Models.SearchResultItem>
        {
            public bool Equals(Data.Models.SearchResultItem x, Data.Models.SearchResultItem y)
            {
                return x.EntityName.Equals(y.EntityName) && x.EntityId.Equals(y.EntityId);
            }

            public int GetHashCode(Data.Models.SearchResultItem obj)
            {
                return obj.EntityName.GetHashCode() * 100 + obj.EntityId.GetHashCode();
            }
        }

        public IEnumerable<SearchResultItem> Search(SearchParams searchParams, out int totalCount, int startIndex = 1, int count = 10)
        {
            var items =
                GetFromCacheOrLoadAndAddToCache(
                    $"SEARCHRESULT:{searchParams.SearchPattern?.ToLower()}:{string.Join(",",searchParams.SearchEntities.Select(x => x.FullName))}",
                    () =>
                    {
                        var dirtyResults = _searchItemGateway.Find(searchParams.SearchPattern, searchParams.SearchEntities).OrderByDescending(x => x.Relevance);
                        var distinctByEntity = dirtyResults.Distinct(new SearchResultEqualityComparer()).ToList();
                        return distinctByEntity;
                    });


            var dataRes = items.OrderByDescending(x => x.Relevance).Skip(startIndex - 1).Take(count);
            totalCount = items.Count;
            return Mapper.Map<IEnumerable<SearchResultItem>>(dataRes);
            
            //IList<SearchResultItemShort> allItems =
            //    GetFromCacheOrLoadAndAddToCache(String.Format("SEARCHRESULT:{0}", searchPattern.ToLower()),
            //        () =>
            //        {
            //            int goodsCount;
            //            var goods = _shopService.GetGoodsIds(searchPattern);
            //            var articles = _articleService.FindArticles(searchPattern);

            //            List<SearchResultItemShort> allFoundItems = new List<SearchResultItemShort>();
            //            foreach (int goodsItem in goods)
            //            {
            //                allFoundItems.Add(new SearchResultItemShort() { ItemId = goodsItem, ItemType = SearchItemType.Goods });
            //            }
            //            foreach (var art in articles)
            //            {
            //                allFoundItems.Add(new SearchResultItemShort() { ItemId = art.ArticleId, ItemType = SearchItemType.Article });
            //            }
            //            return allFoundItems;
            //        });

            //var resultIds = allItems.Skip(startIndex - 1).Take(count);
            //totalCount = allItems.Count;
            //List<SearchResultItem> result = new List<SearchResultItem>();
            //foreach (var item in resultIds)
            //{
            //    SearchResultItem resItem = new SearchResultItem() { ItemId = item.ItemId, ItemType = item.ItemType };
            //    switch (item.ItemType)
            //    {
            //        case SearchItemType.Article:
            //            var art = _articleService.GetArticle(item.ItemId);
            //            resItem.Text = art.Page.Annotation;
            //            resItem.ImageId = art.ImageId;
            //            resItem.Title = art.Page.Title;
            //            resItem.Url = art.Page.RelativeUrl;
            //            break;
            //        case SearchItemType.Goods:
            //            var goodsItem = _shopService.GetGoods(item.ItemId);
            //            resItem.Text = goodsItem.Description;
            //            resItem.Title = goodsItem.Name;
            //            resItem.ImageId = goodsItem.MainImageId;
            //            break;
            //        default:
            //            throw new NotSupportedException();
            //    }
            //    result.Add(resItem);
            //}
            //return result;
        }

    }
}
