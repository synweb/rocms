using System;
using System.Globalization;
using System.IO;
using System.Security.Policy;

namespace jQuery_File_Upload.MVC3.Upload
{

    //TODO: refactor (нужно развести логику для загрузки изображений, файлов на диске и временных файлов)
    public class FilesStatus
    {
        public const string HandlerPath = "/Upload/";

        public const string ControllerPath = "/Gallery/";

        public string file_id { get; set; }
        public string group { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }
        public string progress { get; set; }
        public string url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_url { get; set; }
        public string delete_type { get; set; }
        public string error { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        public FilesStatus() { }

        public FilesStatus(Guid fileId, string fileName)//используется для временных файлов
        {
            file_id = fileId.ToString();
            name = fileName;
        }

        public FilesStatus(FileInfo fileInfo) { SetValues(fileInfo.Name, (int)fileInfo.Length, fileInfo.FullName); }

        public FilesStatus(string fileName, int fileLength, string fullPath) { SetValues(fileName, fileLength, fullPath); }

        public FilesStatus(string fileId, int fileLength)
        {
            name = fileId;
            type = "image/png";
            size = fileLength / 1024;
            progress = "1.0";
            url = ControllerPath + "Image/" + fileId;
            delete_url = "/api/image/remove/" + fileId;
            delete_type = "DELETE";
            thumbnail_url = ControllerPath + "Thumbnail/" + fileId;
        }

        public FilesStatus(string fileId, string fileName, int fileLength) : this(fileId, fileLength)
        {
            //Когда изображение загружается в базу, изначально его title - имя файла
            title = fileName;
        }

        private void SetValues(string fileName, int fileLength, string fullPath)
        {
            name = fileName;
            size = fileLength;
            progress = "1.0";
            url = "/File/Get/" + fileName + "/";
            delete_url = "/File/Delete/" + fileName + "/";
            delete_type = "DELETE";

            var ext = Path.GetExtension(fullPath);
            
            var fileSize = ConvertBytesToMegabytes(new FileInfo(fullPath).Length);
            if (fileSize > 3 || !IsImage(ext)) thumbnail_url = "/Content/admin/vendor/FU/img/generalFile.png";
            else thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath);
        }

        private bool IsImage(string ext)
        {
            return ext == ".gif" || ext == ".jpg" || ext == ".png";
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}