using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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
        private readonly IHeartService _heartService;

        private const string PAGE_CACHE_KEY_TEMPLATE = "Page:{0}";

        private readonly PageGateway _pageGateway = new PageGateway();
        


        public PageService(ISearchService searchService, ILogService logService, IHeartService heartService)
        {
            _searchService = searchService;
            _logService = logService;
            _heartService = heartService;
            InitCache("PageServiceMemoryCache");
            CacheExpirationInMinutes = 30;
        }

        public int CreatePage(Page page)
        {
            using (var ts = new TransactionScope())
            {
                int heartId = _heartService.CreateHeart(page);
                page.HeartId = heartId;
                var newPage = Mapper.Map<Data.Models.Page>(page);
                _pageGateway.Insert(newPage);
                var np = Mapper.Map<Page>(newPage);
                np.CreationDate = DateTime.UtcNow;
                np.CannonicalUrl = _heartService.GetCanonicalUrl(np.RelativeUrl);
                AddOrUpdateCacheObject(GetPageCacheKey(newPage.RelativeUrl), np);
                _searchService.UpdateIndex(np);
                ts.Complete();
                return heartId;
            }
        }

        public Page GetPage(string url)
        {
            string cacheKey = GetPageCacheKey(url);
            return GetFromCacheOrLoadAndAddToCache<Page>(cacheKey, () =>
            {
                var heart = _heartService.GetHeart(url);
                if (heart == null)
                    return null;
                var page = _pageGateway.SelectOne(heart.HeartId);
                var res = Mapper.Map<Page>(page);
                res.FillHeart(heart);
                return res;
            });
            
        }

        public Page GetPage(int id)
        {
            var page = _pageGateway.SelectOne(id);
            if (page == null)
                return null;
            var res = Mapper.Map<Page>(page);
            var heart = _heartService.GetHeart(id);
            Mapper.Map(res, heart);
            return res;
        }
        
        public void UpdatePage(Page page)
        {
            var original = _pageGateway.SelectOne(page.HeartId);
            using (var ts = new TransactionScope())
            {
                _heartService.UpdateHeart(page);
                var dataPage = Mapper.Map<Data.Models.Page>(page);
                _pageGateway.Update(dataPage);
                page.CannonicalUrl = _heartService.GetCanonicalUrl(page.RelativeUrl);
                _searchService.UpdateIndex(page);
                ts.Complete();
            }
            RemoveObjectFromCache(GetPageCacheKey(original.RelativeUrl));
        }

        public void DeletePage(int pageId)
        {
            var heart = _heartService.GetHeart(pageId);
            _heartService.DeleteHeart(pageId);
            RemoveObjectFromCache(GetPageCacheKey(heart.RelativeUrl));
            _searchService.RemoveFromIndex(typeof (Page), pageId);
        }

        public IList<Page> GetPages()
        {
            // забираются полные объекты страниц. если будет работать медленно, можно забирать без определённых полей
            try
            {
                var pages = _pageGateway.Select();
                var res = Mapper.Map<List<Page>>(pages);
                foreach (var page in res)
                {
                    _heartService.Fill(page);
                    page.CannonicalUrl = _heartService.GetCanonicalUrl(page.RelativeUrl);
                }
                return res;
            }
            catch (Exception e)
            {
                // если возникают косяки, нельзя обваливать сайт. возвращаем пустой список.
                _logService.LogError(e);
                return new List<Page>();
            }
        }

        private string GetPageCacheKey(string url)
        {
            return String.Format(PAGE_CACHE_KEY_TEMPLATE, url);
        }

        //TODO: здесь хватит Heart, не нужно возвращать Page
        public IList<Page> GetSitemapPagesInfo()
        {
            return GetPages().Where(x => !x.Noindex).ToList();
        }

        protected override int CacheExpirationInMinutes { get; }
    }
}
