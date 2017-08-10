using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RoCMS.Base.Helpers;
using jQuery_File_Upload.MVC3.Upload;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Upload
{

    public class TempUploadHandler : IHttpHandler
    {
        private readonly JavaScriptSerializer js;

        private ITempFilesService _tempFilesService;

        public TempUploadHandler()
        {
            _tempFilesService = DependencyResolver.Current.GetService<ITempFilesService>();
            js = new JavaScriptSerializer();
            js.MaxJsonLength = 5242880;
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
                try
                {
                    
                    var file = context.Request.Files[i];
                    if (file.ContentLength > AppSettingsHelper.MaxTempFileSizeMb * 1024 * 1024)
                    {
                        throw new InvalidOperationException(String.Format("Размер файла превышает {0} мегабайт", AppSettingsHelper.MaxTempFileSizeMb));
                    }
                    TempFile tf = new TempFile()
                    {
                        FileName = file.FileName,
                        MimeType = file.ContentType,
                        IpAddress = context.Request.UserHostAddress,
                    };
                    using (var reader = new BinaryReader(file.InputStream))
                    {
                        tf.Content = reader.ReadBytes(file.ContentLength);
                    }

                    Guid id = _tempFilesService.AddFile(tf);
                    statuses.Add(new FilesStatus(id, file.FileName));
                }
                catch (InvalidOperationException e)
                {
                    statuses.Add(new FilesStatus() { error = e.Message });
                }

            }
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

            var jsonObj = js.Serialize(statuses.ToArray());
            context.Response.Write(jsonObj);
        }

    }
}