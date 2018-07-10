using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace RoCMS.Base.Services
{
    public abstract class BaseCacheService
    {
        protected abstract int CacheExpirationInMinutes { get; }
        private MemoryCache _cache;


        protected IEnumerable<T> GetElementsFromCache<T>()
        {
            if (_cache == null)
            {
                throw new NullReferenceException("Объект кеша нужно проинициализировать перед использованием");
            }
            return _cache.Where(x => x.Value is T).Select(x => (T)x.Value);
        }

        protected void ClearCache()
        {
            var keys = _cache.Select(x => x.Key).ToList();

            foreach (var key in keys)
            {
                _cache.Remove(key);
            }
        }

        protected void InitCache(string name = null)
        {
            _cache = string.IsNullOrEmpty(name) ? MemoryCache.Default : new MemoryCache(name);
        }

        protected T GetFromCacheOrLoadAndAddToCache<T>(string cacheKey, Func<T> loader)
        {
            if (_cache == null)
            {
                throw new NullReferenceException("Объект кеша нужно проинициализировать перед использованием");
            }
            var cachedObject = _cache[cacheKey];
            if (cachedObject == null)
            {
                cachedObject = loader();
                if (cachedObject != null)
                {
                    _cache.Add(cacheKey, cachedObject, GetCacheItemPolicy());
                }
            }
            return (T)cachedObject;
        }

        private CacheItemPolicy GetCacheItemPolicy() => new CacheItemPolicy() {SlidingExpiration = TimeSpan.FromMinutes(CacheExpirationInMinutes)};

        protected T GetFromCache<T>(string cacheKey)
        {
            if (_cache == null)
            {
                throw new NullReferenceException("Объект кеша нужно проинициализировать перед использованием");
            }
            var cachedObject = _cache[cacheKey];
            if (cachedObject == null)
            {
                throw new NullReferenceException("ОБъект отсутствует в кеше");
            }
            return (T)cachedObject;
        }

        protected bool CacheContainsObject(string cacheKey)
        {
            if (_cache == null)
            {
                throw new NullReferenceException("Объект кеша нужно проинициализировать перед использованием");
            }
            var cachedObject = _cache[cacheKey];
            return cachedObject != null;
        }
    

        protected void AddOrUpdateCacheObject(string cacheKey, object obj)
        {
            if (_cache == null)
            {
                throw new NullReferenceException("Объект кеша нужно проинициализировать перед использованием");
            }
            var cachedObject = _cache[cacheKey];
            //TODO: по виду не очень хорошо с точки зрения многопоточности... могут быть исключения при доступе к обновляемым страницам
            if (cachedObject != null)
            {
                _cache.Remove(cacheKey);
            }
            _cache.Add(cacheKey, obj, GetCacheItemPolicy());
        }

        protected void RemoveObjectFromCache(string cacheKey)
        {
            if (_cache == null)
            {
                throw new NullReferenceException("Объект кеша нужно проинициализировать перед использованием");
            }
            _cache.Remove(cacheKey);
        }

        public void RefreshCacheExpirationForObject(string cacheKey)
        {
            if (_cache == null)
            {
                throw new NullReferenceException("Объект кеша нужно проинициализировать перед использованием");
            }

            var cachedObject = _cache[cacheKey];
            if (cachedObject == null) return;

            _cache.Remove(cacheKey);
            _cache.Add(cacheKey, cachedObject, GetCacheItemPolicy());
        }
    }
}
