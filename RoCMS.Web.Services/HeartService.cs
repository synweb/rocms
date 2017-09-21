using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class HeartService: BaseCoreService, IHeartService
    {
        public HeartService()
        {
            InitCache(nameof(HeartService));
        }

        private readonly HeartGateway _heartGateway = new HeartGateway();
        public string GetCanonicalUrl(string relativeUrl)
        {
            string cacheKey = GetCanonicalUrlCacheKey(relativeUrl);
            return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () =>
            {
                string prefix = GetCanonicalPageUrlPrefix(relativeUrl);
                return String.IsNullOrEmpty(prefix) ? relativeUrl : String.Format("{0}/{1}", prefix, relativeUrl);
            });
        }
        

        public string GetNextAvailableRelativeUrl(string relativeUrl)
        {
            var existingPage = _heartGateway.SelectByRelativeUrl(relativeUrl);
            if (existingPage == null)
                return relativeUrl;
            bool exists;
            string url;
            int counter = 1;
            do
            {
                counter++;
                url = $"{relativeUrl}-{counter}";
                existingPage = _heartGateway.SelectByRelativeUrl(url);
                exists = existingPage != null;
            } while (exists);
            return url;
        }

        public void DeleteHeart(int id)
        {
            var page = _heartGateway.SelectOne(id);
            var children = _heartGateway.SelectChildren(id);
            using (var ts = new TransactionScope())
            {
                foreach (var child in children)
                {
                    child.ParentHeartId = null;
                    _heartGateway.Update(child);
                }
                _heartGateway.Delete(id);
                ts.Complete();
            }
            RemoveObjectFromCache(GetCanonicalUrlCacheKey(page.RelativeUrl));
        }

        public void UpdateHeart(Heart heart)
        {
            var dataHeart = Mapper.Map<Data.Models.Heart>(heart);
            _heartGateway.Update(dataHeart);
            RemoveObjectFromCache(GetCanonicalUrlCacheKey(heart.RelativeUrl));
        }

        public int CreateHeart(Heart heart)
        {
            var dataHeart = Mapper.Map<Data.Models.Heart>(heart);
            int heartId = _heartGateway.Insert(dataHeart);
            return heartId;
        }

        public void Fill(Heart heart)
        {
            if (heart.HeartId == 0)
            {
                throw new Exception();
            }
            var dataHeart = _heartGateway.SelectOne(heart.HeartId);
            Mapper.Map(dataHeart, heart);
        }

        private string GetCanonicalPageUrlPrefix(string relativeUrl)
        {
            string res = String.Empty;
            bool hasParent = false;
            var page = _heartGateway.SelectByRelativeUrl(relativeUrl);
            if (page.ParentHeartId.HasValue)
            {
                var parent = _heartGateway.SelectOne(page.ParentHeartId.Value);
                res = parent.RelativeUrl;
                hasParent = parent.ParentHeartId.HasValue;
            }
            if (String.IsNullOrEmpty(res) || !hasParent)
            {
                return res;
            }

            return String.Format("{0}/{1}", GetCanonicalPageUrlPrefix(res), res);
        }

        private const string CANONICAL_URL_CACHE_KEY = "Cannonical:{0}";
        private string GetCanonicalUrlCacheKey(string url)
        {
            return String.Format(CANONICAL_URL_CACHE_KEY, url);
        }
    }
}
