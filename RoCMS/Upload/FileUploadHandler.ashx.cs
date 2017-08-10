using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using jQuery_File_Upload.MVC3.Upload;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Upload
{

    public class FileUploadHandler : IHttpHandler
    {
        private readonly JavaScriptSerializer js;

        private readonly List<string> _allowedFiles;

        private string StorageRoot
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UploadedFiles/")); } //Path should! always end with '/'
        }

        public FileUploadHandler()
        {
            js = new JavaScriptSerializer();
            js.MaxJsonLength = 41943040;

            var settingService = DependencyResolver.Current.GetService<ISettingsService>();
            var allowedFiles = settingService.GetSettings().AllowedFileExtensions;
            _allowedFiles = allowedFiles.Split(',').ToList();
        }

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("Pragma", "no-cache");
            context.Response.AddHeader("Cache-Control", "private, no-cache");

            HandleMethod(context);
        }

        // Handle request based on method
        private void HandleMethod(HttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                //case "HEAD":
                //case "GET":
                //    if (GivenFilename(context)) DeliverFile(context);
                //    else ListCurrentFiles(context);
                //    break;

                case "POST":
                case "PUT":
                    UploadFile(context);
                    break;

                //case "DELETE":
                //    DeleteFile(context);
                //    break;

                case "OPTIONS":
                    ReturnOptions(context);
                    break;

                default:
                    context.Response.ClearHeaders();
                    context.Response.StatusCode = 405;
                    break;
            }
        }

        private static void ReturnOptions(HttpContext context)
        {
            context.Response.AddHeader("Allow", "POST,PUT,OPTIONS");
            context.Response.StatusCode = 200;
        }

        // Delete file from the server
        private void DeleteFile(HttpContext context)
        {
            var filePath = StorageRoot + context.Request["f"];
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        // Upload file to the server
        private void UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
                

            UploadWholeFile(context, statuses);

            WriteJsonIframeSafe(context, statuses);
        }

        // Upload entire file
        private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                var file = context.Request.Files[i];
                if(file.ContentLength == 0)
                {
                    return;
                }
                if (IsAllowedFile(file.FileName))
                {
                    string fullPath = StorageRoot + file.FileName;
                    file.SaveAs(fullPath);
                    statuses.Add(new FilesStatus(file.FileName, Convert.ToInt32(file.InputStream.Length), fullPath));
                }
                else
                {
                    throw new InvalidOperationException("Загрузка подобного файла запрещена");
                }

            }
        }

        private bool IsAllowedFile(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            return _allowedFiles.Contains(ext);
        }

        private void WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            try
            {
                if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
                    context.Response.ContentType = "application/json";
                else
                    context.Response.ContentType = "text/plain";
            }
            catch
            {
                context.Response.ContentType = "text/plain";
            }
            var jsonRes = new {files = statuses.ToArray()};
            var jsonObj = js.Serialize(jsonRes);
            context.Response.Write(jsonObj);
        }

        private static bool GivenFilename(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Request["f"]);
        }

        private void DeliverFile(HttpContext context)
        {
            var filename = context.Request["f"];
            var filePath = StorageRoot + filename;

            if (File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
                context.Response.StatusCode = 404;
        }

        private void ListCurrentFiles(HttpContext context)
        {
            var files =
                new DirectoryInfo(StorageRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(f => new FilesStatus(f))
                    .ToArray();

            string jsonObj = js.Serialize(files);
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
        }

    }
}