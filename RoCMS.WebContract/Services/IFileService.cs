using System.Collections.Generic;
using System.IO;

namespace RoCMS.Web.Contract.Services
{
    public interface IFileService
    {
        FileInfo GetFile(string fileName);
        List<FileInfo> GetFiles();
        void DeleteFile(string fileName);
    }
}
