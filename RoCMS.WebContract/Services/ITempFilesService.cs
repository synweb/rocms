using System;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface ITempFilesService
    {
        Guid AddFile(TempFile file);
        TempFile GetFile(Guid id);

        void RemoveFile(Guid id);
    }
}
