using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RoCMS.Base.Services;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class InterfaceStringService: BaseCoreService, IInterfaceStringService
    {

        private const string INTERFACE_STRING_CACHE_KEY_TEMPLATE = "ISTR:{0}";

        public InterfaceStringService()
        {
            InitCache("InterfaceStringServiceMemoryCache");
        }
        
        private string GetCacheKey(string key)
        {
            return string.Format(INTERFACE_STRING_CACHE_KEY_TEMPLATE, key);
        }

        private readonly InterfaceStringGateway _interfaceStringGateway = new InterfaceStringGateway();

        protected override int CacheExpirationInMinutes => 30;

        public ICollection<InterfaceString> GetStrings()
        {
            var dataRes = _interfaceStringGateway.Select();
            var res = Mapper.Map<ICollection<InterfaceString>>(dataRes);
            foreach (var str in res)
            {
                AddOrUpdateCacheObject(GetCacheKey(str.Key), str);
            }
            return res;
        }

        public InterfaceString GetString(string key)
        {
            return GetFromCacheOrLoadAndAddToCache(GetCacheKey(key), () =>
            {
                var dataRes = _interfaceStringGateway.SelectOne(key);
                var res = Mapper.Map<InterfaceString>(dataRes);
                return res;
            });
        }

        public void Upsert(InterfaceString interfaceString)
        {
            var dataRec = Mapper.Map<Data.Models.InterfaceString>(interfaceString);
            _interfaceStringGateway.Upsert(dataRec);
            AddOrUpdateCacheObject(GetCacheKey(interfaceString.Key), interfaceString);
        }

        public void Delete(string key)
        {
            _interfaceStringGateway.Delete(key);
            RemoveObjectFromCache(GetCacheKey(key));
        }

        public void SaveMany(ICollection<InterfaceString> strings)
        {
            var oldStrings = GetStrings();
            
            foreach (var newStr in strings)
            {
                if (CacheContainsObject(GetCacheKey(newStr.Key)))
                {
                    var oldStr = oldStrings.Single(x => x.Key.Equals(newStr.Key));
                    if(oldStr.Value == newStr.Value)
                        continue;
                }
                Upsert(newStr);
            }

            foreach (var oldStr in oldStrings.Where(x => strings.All(y => !y.Key.Equals(x.Key))))
            {
                Delete(oldStr.Key);
            }
        }
    }
}
