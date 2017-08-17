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
        private readonly string _storageRoot = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles/"));


        public FileInfo GetFile(string fileName)
        {
            var filePath = _storageRoot + fileName;

            if (File.Exists(filePath))
            {
                return new FileInfo(filePath);
            }
            throw new FileNotFoundException();
        }

        public List<FileInfo> GetFiles()
        {
            return new DirectoryInfo(_storageRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)).ToList();
        }

        public void DeleteFile(string fileName)
        {
            var filePath = _storageRoot + fileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
