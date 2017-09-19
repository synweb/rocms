using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using AutoMapper;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;
using Page = RoCMS.Web.Contract.Models.Page;

namespace RoCMS.Web.Services
{
    public class PageService : BaseCoreService, IPageService
    {
        private readonly ISearchService _searchService;
        private readonly ILogService _logService;

        private const string PAGE_CACHE_KEY_TEMPLATE = "Page:{0}";
        private const string PAGE_CANNONICAL_URL_CACHE_KEY = "PageCannonical:{0}";

        private readonly PageGateway _pageGateway = new PageGateway();


        public PageService(ISearchService searchService, ILogService logService)
        {
            _searchService = searchService;
            _logService = logService;
            InitCache("PageServiceMemoryCache");
            CacheExpirationInMinutes = 30;
        }

        public int CreatePage(Page page)
        {
            var newPage = Mapper.Map<Data.Models.Page>(page);
            newPage.PageId = _pageGateway.Insert(newPage);
            var np = Mapper.Map<Page>(newPage);
            np.CreationDate = DateTime.UtcNow;
            np.CannonicalUrl = GetPageCanonicalUrl(np.RelativeUrl);
            AddOrUpdateCacheObject(GetPageCacheKey(newPage.RelativeUrl), np);
            _searchService.UpdateIndex(np);
            return newPage.PageId;
        }

        public Page GetPage(string url)
        {
            string cacheKey = GetPageCacheKey(url);
            return GetFromCacheOrLoadAndAddToCache<Page>(cacheKey, () =>
            {
                var page = _pageGateway.SelectByRelativeUrl(url);
                var res = Mapper.Map<Page>(page);
                res.CannonicalUrl = GetPageCanonicalUrl(res.RelativeUrl);
                return res;
            });
            
        }

        public Page GetPage(int id)
        {
            var page = _pageGateway.SelectOne(id);
            var res = Mapper.Map<Page>(page);
            res.CannonicalUrl = GetPageCanonicalUrl(res.RelativeUrl);
            return res;
        }

        public IList<PageInfo> GetPagesInfo()
        {
            // забираются полные объекты страниц. если будет работать медленно, можно забирать без определённых полей
            try
            {
                var pages = _pageGateway.Select();
                var res = Mapper.Map<List<PageInfo>>(pages);
                foreach (var pageInfo in res)
                {
                    pageInfo.CannonicalUrl = GetPageCanonicalUrl(pageInfo.RelativeUrl);
                }
                return res;
            }
            catch (Exception e)
            {
                // если возникают косяки, нельзя обваливать сайт. возвращаем пустой список.
                _logService.LogError(e); 
                return new List<PageInfo>();
            }
        }

        public void UpdatePage(Page page)
        {
            var original = _pageGateway.SelectOne(page.PageId);
            var dataPage = Mapper.Map<Data.Models.Page>(page);
            _pageGateway.Update(dataPage);
            page.CannonicalUrl = GetPageCanonicalUrl(page.RelativeUrl);
            _searchService.UpdateIndex(page);
            RemoveObjectFromCache(GetGetPageCanonicalUrlCacheKey(original.RelativeUrl));
            RemoveObjectFromCache(GetPageCacheKey(original.RelativeUrl));
        }

        public void DeletePage(int pageId)
        {
            var page = _pageGateway.SelectOne(pageId);
            var children = _pageGateway.SelectChildren(pageId);
            using (var ts = new TransactionScope())
            {
                _pageGateway.Delete(pageId);
                foreach (var child in children)
                {
                    child.ParentPageId = null;
                    _pageGateway.Update(child);
                }
                ts.Complete();
            }
            RemoveObjectFromCache(GetPageCacheKey(page.RelativeUrl));
            RemoveObjectFromCache(GetGetPageCanonicalUrlCacheKey(page.RelativeUrl));
            _searchService.RemoveFromIndex(typeof (Page), pageId);
        }

        private string GetPageCacheKey(string url)
        {
            return String.Format(PAGE_CACHE_KEY_TEMPLATE, url);
        }

        private string GetGetPageCanonicalUrlCacheKey(string url)
        {
            return String.Format(PAGE_CANNONICAL_URL_CACHE_KEY, url);
        }

        public string GetPageCanonicalUrl(string relativeUrl)
        {
            string cacheKey = GetGetPageCanonicalUrlCacheKey(relativeUrl);
            return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () =>
            {
                string prefix = GetCanonicalPageUrlPrefix(relativeUrl);
                return String.IsNullOrEmpty(prefix) ? relativeUrl : String.Format("{0}/{1}", prefix, relativeUrl);
            });

        }

        public IList<PageInfo> GetSitemapPagesInfo()
        {
            var settingsService = DependencyResolver.Current.GetService<ISettingsService>();
            string mainPage = settingsService.GetHomepageUrl();
            return GetPagesInfo().Where(x => !x.HideInSitemap && x.RelativeUrl != mainPage).ToList();
        }

        public string GetNextAvailableRelativeUrl(string relativeUrl)
        {
            var existingPage = _pageGateway.SelectByRelativeUrl(relativeUrl);
            if (existingPage == null)
                return relativeUrl;
            bool exists;
            string url;
            int counter = 1;
            do
            {
                counter++;
                url = $"{relativeUrl}-{counter}";
                existingPage = _pageGateway.SelectByRelativeUrl(url);
                exists = existingPage != null;
            } while (exists);
            return url;
        }

        private string GetCanonicalPageUrlPrefix(string relativeUrl)
        {
            string res = String.Empty;
            bool hasParent = false;
            var page = _pageGateway.SelectByRelativeUrl(relativeUrl);
            if (page.ParentPageId.HasValue)
            {
                var parent = _pageGateway.SelectOne(page.ParentPageId.Value);
                res = parent.RelativeUrl;
                hasParent = parent.ParentPageId.HasValue;
            }
            if (String.IsNullOrEmpty(res) || !hasParent)
            {
                return res;
            }

            return String.Format("{0}/{1}", GetCanonicalPageUrlPrefix(res), res);
        }

        protected override int CacheExpirationInMinutes { get; }
    }
}
