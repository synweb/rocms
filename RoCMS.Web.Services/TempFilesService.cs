using System;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using RoCMS.Base.Helpers;
using RoCMS.Base.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class TempFilesService : BaseCoreService, ITempFilesService
    {
        

        public TempFilesService()
        {
            
            InitCache("TempFilesServiceMemoryCache");
        }

        protected override int CacheExpirationInMinutes
        {
            get { return AppSettingsHelper.MinutesToExpireTempFileCache; }
        }

        public Guid AddFile(TempFile file)
        {
            //if (GetElementsFromCache<TempFile>().Count(x => x.IpAddress == file.IpAddress) >= AppSettingsHelper.TempFilesCount)
            //    throw new InvalidOperationException(
            //        string.Format("С данного компьютера загружено максимальное количество файлов. Подождите {0} минут и попробуйте снова", CacheExpirationInMinutes)
            //        );


            Guid id = Guid.NewGuid();
            file.Id = id;
            AddOrUpdateCacheObject(id.ToString(), file);
            return id;
        }

        public TempFile GetFile(Guid id)
        {
            try
            {
                return GetFromCache<TempFile>(id.ToString());
            }
            catch
            {
                throw new FileNotFoundException("Файл отсутствует в кэше");
            }

        }

        public void RemoveFile(Guid id)
        {
            try
            {
                RemoveObjectFromCache(id.ToString());
            }
            catch
            {

            }
        }
    }

}
