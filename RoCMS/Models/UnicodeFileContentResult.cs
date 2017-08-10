using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RoCMS.Models
{
    public class UnicodeFileContentResult : ActionResult
    {

        public UnicodeFileContentResult(byte[] fileContents, string contentType)
        {
            if (fileContents == null || string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException();
            }

            FileContents = fileContents;
            ContentType = contentType;
        }

        public UnicodeFileContentResult(byte[] fileContents, string contentType, string fileName): this(fileContents, contentType)
        {
            FileDownloadName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var encoding = UnicodeEncoding.UTF8;
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            response.Clear();
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlEncode(FileDownloadName, encoding)));
            response.ContentType = ContentType;
            response.Charset = encoding.WebName;
            response.HeaderEncoding = encoding;
            response.ContentEncoding = encoding;
            response.BinaryWrite(FileContents);
            response.End();
        }

        public byte[] FileContents { get; private set; }

        public string ContentType { get; private set; }

        public string FileDownloadName { get; set; }
    }
}