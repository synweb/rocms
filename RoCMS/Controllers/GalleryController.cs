using System.IO;
using System.Linq;
using System.Web.UI;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using System;
using System.Web.Mvc;
using RoCMS.Web.Contract.Services;
using System.Globalization;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IAlbumService _albumService;
        private readonly ISettingsService _settingsService;
        private readonly ILogService _logService;

        public GalleryController(IImageService imageService, IAlbumService albumService, ISettingsService settingsService, ILogService logService)
        {
            _imageService = imageService;
            _albumService = albumService;
            _settingsService = settingsService;
            _logService = logService;
        }

        //
        // GET: /Gallery/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return new RedirectResult("/", true);
        }

        [AllowAnonymous]
        [ActionName("Image")]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "id", Location = OutputCacheLocation.Client)]
        public ActionResult GetImage(string id)
        {
            return GetPicture(null, id);
        }

        [AllowAnonymous]
        [ActionName("Thumbnail")]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "id", Location = OutputCacheLocation.Client)]
        public ActionResult GetThumbnail(string id, int? w = null, int? h = null)
        {
            if (w == null && h == null)
            {
                var size = _imageService.GetSmallestThumbnailSize();
                return GetPicture(size, id);
            }
            if (w.HasValue)
            {
                // задана макс. ширина
                // в приоритете - w. h игнорируем.
                var size = new ThumbnailSize(w.Value, ImageSide.Width);
                return GetPicture(size, id);
            }
            else
            {
                // ширины нет, задана макс. высота
                var size = new ThumbnailSize(h.Value, ImageSide.Height);
                return GetPicture(size, id);
            }
        }

        private ActionResult GetPicture(ThumbnailSize? size, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ImageNotFound();
            }
            string imageId = id.ToLower();
            try
            {
                var path = _imageService.GetImagePath(imageId, size, true);
                if (System.IO.File.Exists(path))
                {
                    return File(path, GetImageMimetype(path));
                }
                return ImageNotFound();
            }
            catch (Exception)
            {
                return ImageNotFound();
            }
        }

        private ActionResult ImageNotFound()
        {
            return File(Url.Content("~/Content/base/ro/img/img-not-found.gif"), "image/gif");
        }

        private string GetImageMimetype(string path)
        {
            var arr = path.Split('.');
            return ExtenstionToMimetype(arr.Last());
        }

        private string ExtenstionToMimetype(string ext)
        {
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return "image/jpeg";
                case "png":
                    return "image/png";
                case "gif":
                    return "image/gif";
            }
            throw new ArgumentException("Not an image mimetype");
        }

        private void ClearAlbumOutputCache(int albumId)
        {
            var images = _albumService.GetAlbumImages(albumId);
            foreach (var albumImageInfo in images)
            {
                //  Get the url for the action method:
                var staleItem = Url.RouteUrl("Image", new
                {
                    id = albumImageInfo.ImageId,
                });
                Response.RemoveOutputCacheItem(staleItem);
                var staleItem2 = Url.RouteUrl("Thumbnail", new
                {
                    id = albumImageInfo.ImageId,
                });
                Response.RemoveOutputCacheItem(staleItem2);
            }
        }

        [System.Web.Http.HttpPost]
        public JsonResult SetAlbumWatermark(int albumId, string watermarkImageId)
        {
            try
            {
                _albumService.SetAlbumWatermark(albumId, watermarkImageId);
                ClearAlbumOutputCache(albumId);
                return Json(ResultModel.Success);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return Json(new ResultModel(e));
            }
        }

        [System.Web.Http.HttpPost]
        public JsonResult RemoveAlbumWatermark(int albumId)
        {
            try
            {
                _albumService.SetAlbumWatermark(albumId, null);
                ClearAlbumOutputCache(albumId);
                return Json(ResultModel.Success);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return Json(new ResultModel(e));
            }
        }

        [AuthorizeResources(RoCmsResources.Albums)]
        public JsonResult RemoveImage(string id)
        {
            _imageService.RemoveImage(id);
            return Json(new ResultModel(true, id));
        }

        public ActionResult Album(int id)
        {
            return PartialView(id);
        }

        [AllowAnonymous]
        public JsonResult RandomAlbumImage(int albumId)
        {
            string imageId = _albumService.GetRandomImageFromAlbum(albumId);
            return Json(new ResultModel(true, new { ImageId = imageId, ImageUrl = Url.Action("Image", "Gallery", new { id = imageId }) }));
        }

        [AuthorizeResources(RoCmsResources.Albums)]
        public ActionResult PickImageDialog()
        {
            return PartialView();
        }

        [AuthorizeResources(RoCmsResources.Albums)]
        public ActionResult PickImageAlbum()
        {
            var albums = _albumService.GetAlbums();
            return PartialView(albums);
        }

        [AuthorizeResources(RoCmsResources.Albums)]
        public ActionResult PickImageFromAlbum(int id, bool multiple = false)
        {
            ViewBag.IsMultiple = multiple;
            var imgIds = _albumService.GetAlbumImages(id);
            ViewBag.AlbumId = id;
            return PartialView(imgIds);
        }
    }
}
