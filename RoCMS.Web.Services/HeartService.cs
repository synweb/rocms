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
            RenewCanonicalUrlTable();
        }

        private void RenewCanonicalUrlTable()
        {
            var hearts = _heartGateway.Select();
            _heartCanonicalUrls = hearts.ToDictionary(x => x.HeartId, x => GetUncachedCanonicalUrl(x.RelativeUrl));
        }

        private string GetUncachedCanonicalUrl(string relativeUrl)
        {
            string prefix = GetCanonicalUrlPrefix(relativeUrl);
            return String.IsNullOrEmpty(prefix) ? relativeUrl : String.Format("{0}/{1}", prefix, relativeUrl);
        }

        private IDictionary<int, string> _heartCanonicalUrls;

        private readonly HeartGateway _heartGateway = new HeartGateway();
        public string GetCanonicalUrl(string relativeUrl)
        {
            string cacheKey = GetCanonicalUrlCacheKey(relativeUrl);
            return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () => GetUncachedCanonicalUrl(relativeUrl));
        }
        
        public string GetNextAvailableRelativeUrl(string relativeUrl)
        {
            var existingHeart = _heartGateway.SelectByRelativeUrl(relativeUrl);
            if (existingHeart == null)
                return relativeUrl;
            bool exists;
            string url;
            int counter = 1;
            do
            {
                counter++;
                url = $"{relativeUrl}-{counter}";
                existingHeart = _heartGateway.SelectByRelativeUrl(url);
                exists = existingHeart != null;
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

        public string GetCanonicalUrl(int heartId)
        {
            string cacheKey = GetCanonicalUrlCacheKey(heartId);
            return GetFromCacheOrLoadAndAddToCache<string>(cacheKey, () =>
            {
                var heart = _heartGateway.SelectOne(heartId);
                return GetUncachedCanonicalUrl(heart.RelativeUrl);
            });
        }

        public Heart GetHeart(int heartId)
        {
            var dataRes = _heartGateway.SelectOne(heartId);
            if (dataRes == null)
                return null;
            var res = Mapper.Map<Heart>(dataRes);
            res.CannonicalUrl = GetCanonicalUrl(res.RelativeUrl);
            return res;
        }

        public ICollection<Heart> GetHearts()
        {
            var dataRes = _heartGateway.Select();
            var res = Mapper.Map<ICollection<Heart>>(dataRes);
            foreach (var heart in res)
            {
                heart.CannonicalUrl = GetCanonicalUrl(heart.HeartId);
            }
            return res;
        }

        public bool CheckIfUrlExists(string relativeUrl)
        {
            var heart = _heartGateway.SelectByRelativeUrl(relativeUrl);
            return heart != null;
        }

        public Heart GetHeart(string relativeUrl)
        {
            var dataRes = _heartGateway.SelectByRelativeUrl(relativeUrl);
            if (dataRes == null)
                return null;
            var res = Mapper.Map<Heart>(dataRes);
            res.CannonicalUrl = GetCanonicalUrl(res.RelativeUrl);
            return res;
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

        private string GetCanonicalUrlPrefix(string relativeUrl)
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

            return String.Format("{0}/{1}", GetCanonicalUrlPrefix(res), res);
        }

        private const string CANONICAL_URL_CACHE_KEY = "Cannonical:{0}";
        private const string CANONICAL_URL_CACHE_KEY_BYID = "Cannonical:#{0}";
        private string GetCanonicalUrlCacheKey(string url)
        {
            return String.Format(CANONICAL_URL_CACHE_KEY, url);
        }
        private string GetCanonicalUrlCacheKey(int id)
        {
            return String.Format(CANONICAL_URL_CACHE_KEY_BYID, id);
        }
    }
}
