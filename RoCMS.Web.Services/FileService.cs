using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class FileService: IFileService
    {

        private string StorageRoot
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles/")); } //Path should! always end with '/'
        }



        public FileInfo GetFile(string fileName)
        {
            var filePath = StorageRoot + fileName;

            if (File.Exists(filePath))
            {
                return new FileInfo(filePath);
            }
            throw new FileNotFoundException();
        }

        public List<FileInfo> GetFiles()
        {
            return new DirectoryInfo(StorageRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToList();
        }

        public void DeleteFile(string fileName)
        {
            var filePath = StorageRoot + fileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
