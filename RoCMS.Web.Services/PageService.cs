﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using AutoMapper;
using RoCMS.Base.Exceptions;
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
            //Reindex();
        }

        public void Reindex()
        {
            Task.Run(() =>
            {
                try
                {
                    _logService.TraceMessage("Reindex started: pages");
                    // goods
                    var pages = GetPages();
                    _logService.TraceMessage($"Reindexing {pages.Count} pages");
                    foreach (var page in pages)
                    {
                        _searchService.UpdateIndex(page);
                        _logService.TraceMessage($"Reindexed Page {page.HeartId}");
                    }
                    _logService.TraceMessage("Reindex OK");
                }
                catch (Exception e)
                {
                    _logService.LogError(e);
                }
            });
        }

        public int CreatePage(Page page)
        {
            page.Type = page.GetType().FullName;
            int heartId;
            using (var ts = new TransactionScope())
            {
                heartId = _heartService.CreateHeart(page);
                page.HeartId = heartId;
                var newPage = Mapper.Map<Data.Models.Page>(page);
                _pageGateway.Insert(newPage);
                page.CanonicalUrl = _heartService.GetCanonicalUrl(page.RelativeUrl);
                _searchService.UpdateIndex(page);
                ts.Complete();
            }
            AddOrUpdateCacheObject(GetPageCacheKey(page.RelativeUrl), page);
            return heartId;
        }

        public Page GetPage(string url)
        {
            string cacheKey = GetPageCacheKey(url);
            return GetFromCacheOrLoadAndAddToCache<Page>(cacheKey, () =>
            {
                var heart = _heartService.GetHeart(url);
                if (heart == null)
                    throw new UrlNotFoundException(url);
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
                throw new UrlNotFoundException();
            var res = Mapper.Map<Page>(page);
            var heart = _heartService.GetHeart(id);
            res.FillHeart(heart);
            return res;
        }
        
        public void UpdatePage(Page page)
        {
            var original = _heartService.GetHeart(page.HeartId);
            using (var ts = new TransactionScope())
            {
                _heartService.UpdateHeart(page);
                var dataPage = Mapper.Map<Data.Models.Page>(page);
                _pageGateway.Update(dataPage);
                page.CanonicalUrl = _heartService.GetCanonicalUrl(page.RelativeUrl);
                _searchService.UpdateIndex(page);
                ts.Complete();
            }
            RemoveObjectFromCache(GetPageCacheKey(original.RelativeUrl));
        }

        public void DeletePage(int pageId)
        {
            using (var ts = new TransactionScope())
            {
                var heart = _heartService.GetHeart(pageId);
                _heartService.DeleteHeart(pageId);
                RemoveObjectFromCache(GetPageCacheKey(heart.RelativeUrl));
                _searchService.RemoveFromIndex(typeof (Page), pageId);

                ts.Complete();
            }
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
                    page.CanonicalUrl = _heartService.GetCanonicalUrl(page.RelativeUrl);
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
            var settingsService = DependencyResolver.Current.GetService<ISettingsService>();
            string mainPage = settingsService.GetHomepageUrl();
            return GetPages().Where(x => !x.Noindex && x.RelativeUrl != mainPage).ToList();
        }

        protected override int CacheExpirationInMinutes { get; }
    }
}
