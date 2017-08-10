using System.Collections.Generic;
using System.IO;
using System.Web;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IImageService
    {
        string UploadFile(HttpPostedFile file);
        string UploadImage(Image image, string filename);
        void RemoveImage(string imageId);
        IEnumerable<ImageInfo> GetAllImageInfos();
        IEnumerable<ImageInfo> GetGalleryImageInfos();
        string GetImagePath(string imageId, ThumbnailSize? size, bool createThumbnailIfNotExists = false);
        ImageInfo GetImageInfo(string imageId);
        void ClearUnusedThumbnailDirectories();
        ThumbnailSize? GetSmallestThumbnailSize();
    }
}
