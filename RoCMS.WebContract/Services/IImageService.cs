using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        void ApplyWatermark(string imageId, string watermarkImageId);
        void RestoreImage(string imageId);
        Task<string> DownloadImage(string url);
    }
}
