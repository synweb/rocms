using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Infrastructure;
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
            var groups = hearts.GroupBy(x => x.Type);
            _heartUrlPairs = new Dictionary<string, List<UrlPair>>();
            foreach (var @group in groups)
            {
                _heartUrlPairs.Add(group.Key,
                    group.Select(x => new UrlPair(x.RelativeUrl, GetCanonicalUrl(x.RelativeUrl))).ToList());
            }
        }

        private string GetUncachedCanonicalUrl(string relativeUrl)
        {
            string prefix = GetCanonicalUrlPrefix(relativeUrl);
            return String.IsNullOrEmpty(prefix) ? relativeUrl : $"{prefix}/{relativeUrl}";
        }

        private IDictionary<string, List<UrlPair>> _heartUrlPairs;

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
            var heart = _heartGateway.SelectOne(id);
            var children = _heartGateway.SelectChildren(id);
            using (var ts = new TransactionScope())
            {
                DeleteRoute(heart);
                foreach (var child in children)
                {
                    child.ParentHeartId = null;
                    _heartGateway.Update(child);
                }
                _heartGateway.Delete(id);
                ts.Complete();
            }
            RemoveObjectFromCache(GetCanonicalUrlCacheKey(heart.RelativeUrl));
            
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
            res.CanonicalUrl = GetCanonicalUrl(res.RelativeUrl);
            return res;
        }

        public ICollection<UrlPair> GetHeartUrls(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException(nameof(type));
            }
            string typeName = type.FullName;
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(nameof(type));
            }
            if (_heartUrlPairs.ContainsKey(typeName))
            {
                return _heartUrlPairs[typeName];
            }
            return new List<UrlPair>();
        }


        public ICollection<Heart> GetHearts()
        {
            var dataRes = _heartGateway.Select();
            var res = Mapper.Map<ICollection<Heart>>(dataRes);
            foreach (var heart in res)
            {
                heart.CanonicalUrl = GetCanonicalUrl(heart.HeartId);
            }
            return res;
        }

        public ICollection<Heart> GetHearts(IEnumerable<int> heartIds)
        {
            var dataRes = _heartGateway.SelectByIds(heartIds);
            var res = Mapper.Map<ICollection<Heart>>(dataRes);
            foreach (var heart in res)
            {
                heart.CanonicalUrl = GetCanonicalUrl(heart.HeartId);
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
            res.CanonicalUrl = GetCanonicalUrl(res.RelativeUrl);
            return res;
        }

        public void UpdateHeart(Heart heart)
        {
            var originalHeart = _heartGateway.SelectOne(heart.HeartId);
            var dataHeart = Mapper.Map<Data.Models.Heart>(heart);
            _heartGateway.Update(dataHeart);
            RemoveObjectFromCache(GetCanonicalUrlCacheKey(heart.RelativeUrl));
            // удаляем маршрут старого heart
            DeleteRoute(originalHeart);
            // добавляем новый
            CreateRoute(heart);
        }

        private void CreateRoute(Heart heart)
        {
            List<UrlPair> typeRoutes;
            if (!_heartUrlPairs.ContainsKey(heart.Type))
            {
                typeRoutes = new List<UrlPair>();
                _heartUrlPairs.Add(heart.Type, typeRoutes);
            }
            else
            {
                typeRoutes = _heartUrlPairs[heart.Type];
            }
            typeRoutes.Add(new UrlPair(heart.RelativeUrl.ToLower(), GetUncachedCanonicalUrl(heart.RelativeUrl)));
        }
        
        private void DeleteRoute(Data.Models.Heart heart)
        {
            var typeRoutes = _heartUrlPairs[heart.Type];
            typeRoutes.RemoveAll(x => x.RelativeUrl.Equals(heart.RelativeUrl,
                StringComparison.InvariantCultureIgnoreCase));
        }

        public int CreateHeart(Heart heart)
        {
            var dataHeart = Mapper.Map<Data.Models.Heart>(heart);
            int heartId = _heartGateway.Insert(dataHeart);
            CreateRoute(heart);
            return heartId;
        }

        public void Fill(Heart heart)
        {
            if (heart.HeartId == 0)
            {
                throw new ArgumentException(nameof(Heart.HeartId));
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

            return $"{GetCanonicalUrlPrefix(res)}/{res}";
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
