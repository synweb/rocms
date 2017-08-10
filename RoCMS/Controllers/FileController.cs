using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Controllers
{
    public class FileController : Controller
    {
        private IFileService _fileService;
        private ILogService _logService;
        public FileController(IFileService fileService, ILogService logService)
        {
            _fileService = fileService;
            _logService = logService;
        }

        [AuthorizeResources(RoCmsResources.UploadFiles)]
        public ActionResult Index()
        {
            var files = _fileService.GetFiles();
            return View("Files", files);
        }

        [HttpPost]
        [AuthorizeResources(RoCmsResources.UploadFiles)]
        public void DeleteFile(string fileName, string ext)
        {
            var fn = fileName + "." + ext;
            try
            {
                _logService.TraceMessage("Trying to delete " + fn);
                _fileService.DeleteFile(fn);
            }
            catch (Exception e)
            {
                _logService.TraceMessage("Failed to delete " + fn);
                _logService.LogError(e);
            }
        }
        
        [AllowAnonymous]
        public ActionResult Get(string fileName, string ext)
        {
            var fn = fileName + "." + ext;
            try
            {
                _logService.TraceMessage("Trying to get " + fn);
                var file = _fileService.GetFile(fn);

                return new UnicodeFileContentResult(System.IO.File.ReadAllBytes(file.FullName), MimeMapping.GetMimeMapping(file.Name), fn);
            }
            catch (FileNotFoundException e)
            {
                _logService.TraceMessage("Failed to get " + fn);
                _logService.LogError(e);
                return new EmptyResult();
            }
        }
    }
}
