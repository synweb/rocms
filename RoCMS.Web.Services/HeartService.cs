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
    public class HeartService : BaseCoreService, IHeartService
    {
        protected override int CacheExpirationInMinutes => 320; //8h

        public HeartService()
        {
            InitCache(nameof(HeartService));
            RenewCanonicalUrlTable();
        }

        private void RenewCanonicalUrlTable()
        {
            var hearts = _heartGateway.Select();
            Dictionary<int, string> canonicals = new Dictionary<int, string>();
            FillChildrenWithCanonicalUrls(hearts, null, canonicals);


            var groups = hearts.GroupBy(x => x.Type);
            // grouped by type
            _heartUrlPairs = new Dictionary<string, IDictionary<string,string>>();
            foreach (var @group in groups)
            {
                var urls = group.ToDictionary(x => x.RelativeUrl, x => canonicals[x.HeartId], StringComparer.InvariantCultureIgnoreCase);//Select(x => new KeyValuePair<string,string>(x.RelativeUrl, canonicals[x.HeartId])).ToList();
                _heartUrlPairs.Add(group.Key, urls);
            }
        }

        private void FillChildrenWithCanonicalUrls(ICollection<Data.Models.Heart> hearts, int? parentId, Dictionary<int, string> canonicals)
        {
            var currentHearts = hearts.Where(x => parentId == x.ParentHeartId);
            foreach (var heart in currentHearts)
            {
                canonicals.Add(heart.HeartId, GetCanonicalUrl(heart, hearts));
                FillChildrenWithCanonicalUrls(hearts, heart.HeartId, canonicals);
            }
        }

        private string GetCanonicalUrl(Data.Models.Heart heart, ICollection<Data.Models.Heart> hearts)
        {
            string result = heart.RelativeUrl;
            if (heart.ParentHeartId.HasValue)
            {
                result = $"{GetCanonicalUrl(hearts.Single(x => x.HeartId == heart.ParentHeartId), hearts)}/{result}";
            }
            string cacheIntKey = GetCanonicalUrlCacheKey(heart.HeartId);
            string cacheUrlKey = GetCanonicalUrlCacheKey(heart.RelativeUrl);

            AddOrUpdateCacheObject(cacheUrlKey, result);
            AddOrUpdateCacheObject(cacheIntKey, result);

            return result;
        }

        private IDictionary<string,string> CanonicalUrlPairs
        {
            get
            {
                var result = new Dictionary<string, string>();
                foreach (var val in _heartUrlPairs.Values)
                {
                    foreach (var kvp in val)
                    {
                        result.Add(kvp.Key, kvp.Value);
                    }
                }
                return result;
            }
        }

        private string GetUncachedCanonicalUrl(string relativeUrl)
        {
            string prefix = GetCanonicalUrlPrefix(relativeUrl);
            return String.IsNullOrEmpty(prefix) ? relativeUrl : $"{prefix}/{relativeUrl}";
        }

        /// <summary>
        /// Grouped by type.
        /// {type: {relativeUrl: canonicalUrl}}
        /// </summary>
        private IDictionary<string, IDictionary<string,string>> _heartUrlPairs;

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
                RemoveChildrenCachedCanonicalUrl(id);
                _heartGateway.Delete(id);
                ts.Complete();
            }
            RemoveObjectFromCache(GetCanonicalUrlCacheKey(heart.RelativeUrl));
            RemoveObjectFromCache(GetCanonicalUrlCacheKey(heart.HeartId));
        }

        private void RemoveChildrenCachedCanonicalUrl(int parentHeartId)
        {
            var children = _heartGateway.SelectChildren(parentHeartId);
            foreach (var heart in children)
            {
                RemoveObjectFromCache(GetCanonicalUrlCacheKey(heart.RelativeUrl));
                RemoveObjectFromCache(GetCanonicalUrlCacheKey(heart.HeartId));

                DeleteRoute(heart);
                CreateRoute(Mapper.Map<Heart>(heart));

                RemoveChildrenCachedCanonicalUrl(heart.HeartId);
            }
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

        public IDictionary<string, string> GetHeartUrls(Type type)
        {
            lock (this)
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
                if (!_heartUrlPairs.ContainsKey(typeName))
                {
                    _heartUrlPairs.Add(typeName, new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase));
                }

                return _heartUrlPairs[typeName];
            }
        }


        public ICollection<Heart> GetHearts()
        {
            var dataRes = _heartGateway.Select();
            var res = Mapper.Map<ICollection<Heart>>(dataRes);
            foreach (var heart in res)
            {
                heart.CanonicalUrl = CanonicalUrlPairs.ContainsKey(heart.RelativeUrl)
                    ? CanonicalUrlPairs[heart.RelativeUrl]
                    : GetCanonicalUrl(heart.HeartId); 
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

        public ICollection<Heart> GetHearts(string type)
        {
            var hearts = GetHearts();
            return hearts.Where(x => x.Type == type).ToList();
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
            heart.Type = originalHeart.Type; //небольшие костыли для товаров и категорий
            var dataHeart = Mapper.Map<Data.Models.Heart>(heart);
            _heartGateway.Update(dataHeart);
            

            if (originalHeart.RelativeUrl != heart.RelativeUrl || originalHeart.ParentHeartId != heart.ParentHeartId)
            {
                RemoveObjectFromCache(GetCanonicalUrlCacheKey(heart.RelativeUrl));
                RemoveObjectFromCache(GetCanonicalUrlCacheKey(originalHeart.RelativeUrl));
                RemoveObjectFromCache(GetCanonicalUrlCacheKey(heart.HeartId));

                RemoveChildrenCachedCanonicalUrl(heart.HeartId);

                // удаляем маршрут старого heart
                DeleteRoute(originalHeart);
                // добавляем новый
                CreateRoute(heart);
            }

        }

        private void CreateRoute(Heart heart)
        {
            IDictionary<string, string> typeRoutes;
            if (!_heartUrlPairs.ContainsKey(heart.Type))
            {
                typeRoutes = new Dictionary<string, string>();
                _heartUrlPairs.Add(heart.Type, typeRoutes);
            }
            else
            {
                typeRoutes = _heartUrlPairs[heart.Type];
            }
            typeRoutes.Add(heart.RelativeUrl.ToLower(), GetUncachedCanonicalUrl(heart.RelativeUrl));
        }

        private void DeleteRoute(Data.Models.Heart heart)
        {
            var typeRoutes = _heartUrlPairs[heart.Type];
            typeRoutes.Remove(heart.RelativeUrl);
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
