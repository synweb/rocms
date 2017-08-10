using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using RoCMS.Base.Helpers;
using System.Drawing;
using System.Security.AccessControl;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Extensions;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;
using Image = System.Drawing.Image;

namespace RoCMS.Web.Services
{
    public class ImageService: BaseCoreService, IImageService
    {
        private readonly ILogService _logService;
        private readonly ISettingsService _settingsService;

        //private const string THUMBNAIL_CACHE_KEY_TEMPLATE = "Thumbnail:{0}:{1}";

        private const string MIMETYPE_JPG = "image/jpeg";
        private const string MIMETYPE_PNG = "image/png";
        private const string MIMETYPE_GIF = "image/gif";

        private const string IMAGES_DIR = "Images";
        private const string THUMBNAILS_DIR = "Thumbnails";
        private readonly ImageGateway _imageGateway = new ImageGateway();
        private readonly ImageInAlbumGateway _imageInAlbumGateway = new ImageInAlbumGateway();

        public ImageService(ILogService logService, ISettingsService settingsService)
        {
            _logService = logService;
            _settingsService = settingsService;
            //InitCache("ImageServiceMemoryCache");
        }
        
        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="file"></param>
        /// <returns>идентификатор загруженного файла в базе</returns>
        public string UploadFile(HttpPostedFile file)
        {
            var mime = file.ContentType;
            if (mime == MIMETYPE_JPG ||
                mime == MIMETYPE_PNG ||
                mime == MIMETYPE_GIF)
            {
                byte[] bytes = new byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength);

                var img = new Contract.Models.Image()
                {
                    Content = bytes,
                    ContentType = file.ContentType
                };
                return UploadImage(img, file.FileName);
            }
            return string.Empty;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private ImageFormat ContentTypeToImageFormat(string contentType)
        {
            switch (contentType)
            {
                case MIMETYPE_JPG:
                    return ImageFormat.Jpeg;
                case MIMETYPE_GIF:
                    return ImageFormat.Gif;
                case MIMETYPE_PNG:
                    return ImageFormat.Png;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        private ImageFormat ExtensionToImageFormat(string extension)
        {
            switch (extension)
            {
                case "jpg":
                    return ImageFormat.Jpeg;
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "gif":
                    return ImageFormat.Gif;
                case "png":
                    return ImageFormat.Png;
                default:
                    throw new ArgumentException("extension: " + extension);
            }
        }


        private ImageFormat FilenameToImageFormat(string filename)
        {
            string extenstion = filename.Split('.').Last().ToLower();
            return ExtensionToImageFormat(extenstion);
        }

        private System.Drawing.Image GetBitmapImageOfDesiredSize(byte[] imageContent, int maxWidth, int maxHeight)
        {

            System.Drawing.Image img;
            using (var ms = new MemoryStream(imageContent))
            {
                img = Image.FromStream(ms);
                CorrectRotation(img);

                if (img.Height <= maxHeight && img.Width <= maxWidth)
                {
                    Image temp = new Bitmap(img.Width, img.Height);

                    using (var g = Graphics.FromImage(temp))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(img, 0, 0, temp.Width, temp.Height);
                    }
                    img = (Image)temp.Clone();
                }
                else
                {
                    // Масштабирование по высоте
                    if (img.Height > maxHeight)
                    {
                        float scale = maxHeight / (float)img.Height;
                        Image temp = new Bitmap((int)(img.Width * scale), maxHeight);

                        using (var g = Graphics.FromImage(temp))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.DrawImage(img, 0, 0, temp.Width, temp.Height);
                        }
                        img = (Image)temp.Clone();
                    }

                    // Масштабирование по ширине
                    if (img.Width > maxWidth)
                    {
                        float scale = maxWidth / (float)img.Width;
                        Image temp = new Bitmap(maxWidth, (int)(img.Height * scale));

                        using (var g = Graphics.FromImage(temp))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.DrawImage(img, 0, 0, temp.Width, temp.Height);
                        }
                        img = (Image)temp.Clone();
                    }
                }
            }
            return img;
        }

        public string UploadImage(Contract.Models.Image image, string filename)
        {
            int imgHeight = _settingsService.GetSettings<int>(nameof(Setting.ImageMaxHeight));
            int imgWidth = _settingsService.GetSettings<int>(nameof(Setting.ImageMaxWidth));
            var img = GetBitmapImageOfDesiredSize(image.Content, imgWidth, imgHeight);

            string extenstion = filename.Split('.').Last().ToLower();
            string id = GenerateImageId(extenstion);

            _imageGateway.Insert(new Data.Models.Image() {
                ImageId = id,
                InitialFilename = filename
            });
            ImageFormat format= ExtensionToImageFormat(extenstion);


            string imgPath = GetImagePathInternal(id, null);
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IMAGES_DIR));
            CreateDirectoryForPath(imgPath);
            using (Bitmap tempImage = new Bitmap(img))
            {
                tempImage.Save(imgPath, format);
            }

            return id;
        }

        private string GenerateImageId(string extension)
        {
            const int RANDOM_CHAR_COUNT = 25;
            string imageId = string.Empty;
            do
            {
                var sb = new StringBuilder();
                for (int i = 0; i < RANDOM_CHAR_COUNT; i++)
                {
                    int index = RandomHelper.GetRandom(_imageIdChars.Length);
                    sb.Append(_imageIdChars[index]);
                }
                sb.Append('.');
                sb.Append(extension);
                imageId = sb.ToString();
                // Вероятность встретить два одинаковых айдишника - 0.0000....6, где число нулей - 40. 40 нулей, Карл.
            } while (File.Exists(GetImagePathInternal(imageId, null)));
            return imageId;
        }

        private static readonly char[] _imageIdChars = "1234567890_qwertyuiopasdfghjklzxcvbnm".ToCharArray();

        private void CorrectRotation(Image img)
        {
            if (Array.IndexOf(img.PropertyIdList, 274) > -1)
            {
                var orientation = (int)img.GetPropertyItem(274).Value[0];
                switch (orientation)
                {
                    case 1:
                        // No rotation required.
                        break;
                    case 2:
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        img.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        img.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
                // This EXIF data is now invalid and should be removed.
                img.RemovePropertyItem(274);
            }
        }

        private void CreateDirectoryForPath(string imgPath)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (!imgPath.Contains(baseDir))
            {
                //TODO
                throw new Exception();
            }
            var relativePath = imgPath.Replace(baseDir, "");

            var dirNameList = relativePath.Split('\\').ToList();
            dirNameList.Remove(dirNameList.Last()); // удаляем название файла из списка директорий
            for (var i = 0; i < dirNameList.Count; i++)
            {
                var directoryPathArray = new string[i+2];
                directoryPathArray[0] = baseDir;
                dirNameList.CopyTo(0, directoryPathArray, 1, i + 1);
                var directoryPath = Path.Combine(directoryPathArray);
                if (!Directory.Exists(directoryPath))
                {
                    _logService.TraceMessage($"Creating dir {directoryPath}");
                    Directory.CreateDirectory(directoryPath);
                }
            }
        }

        /// <summary>
        /// Получить путь к изображению для отображения
        /// </summary>
        /// <param name="imageId">ID изображения</param>
        /// <param name="size">Размер миниатюры</param>
        /// <param name="createThumbnailIfNotExists">Создать миниатюру, если её нет</param>
        /// <returns></returns>
        public string GetImagePath(string imageId, ThumbnailSize? size, bool createThumbnailIfNotExists=false)
        {
            var path = GetImagePathInternal(imageId, size);
            if (size == null)
                return path;
            if (!createThumbnailIfNotExists || File.Exists(path))
                return path;
            // создать миниатюру, если она не существует. и её не существует
            CreateThumbnail(imageId, size.Value, path);
            return path;
        }

        private void CreateThumbnail(string imageId, ThumbnailSize desiredSize, string thumbnailPath)
        {
            var size = GetNearestThumbnailSize(desiredSize).Value; // ближайший размер точно есть
            var imagePath = GetImagePathInternal(imageId, null);
            var image = File.ReadAllBytes(imagePath);
            Image thumb;
            switch (size.Side)
            {
                case ImageSide.Height:
                    thumb = GetBitmapImageOfDesiredSize(image, int.MaxValue, size.Pixels);
                    break;
                case ImageSide.Width:
                    thumb = GetBitmapImageOfDesiredSize(image, size.Pixels, int.MaxValue);
                    break;
                default:
                    throw new NotImplementedException("Неслыханно-невиданно!");
            }
            CreateDirectoryForPath(thumbnailPath);
            var format = FilenameToImageFormat(imageId);
            using (Bitmap tempImage = new Bitmap(thumb))
            {
                tempImage.Save(thumbnailPath, format);
            }
        }

        public ImageInfo GetImageInfo(string imageId)
        {
            var img = _imageGateway.SelectOne(imageId);
            var fileInfo = new FileInfo(GetImagePathInternal(imageId, null));
            var size = fileInfo.Length; 
            var res = new ImageInfo()
            {
                ImageId = imageId,
                AlbumCount = _imageInAlbumGateway.SelectByImage(imageId).Count,
                CreationDate = img.CreationDate,
                Size = size
            };
            return res;
        }

        public void ClearUnusedThumbnailDirectories()
        {
            var sizesSetting = _settingsService.GetSettings<string>(nameof(Setting.ThumbnailSizes));
            var sizes = sizesSetting?.Split(',') ?? new string[0];
            var thumbDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, THUMBNAILS_DIR);
            if(!Directory.Exists(thumbDirPath))
                return;
            var subDirs = Directory.GetDirectories(thumbDirPath);
            foreach (var subDir in subDirs)
            {
                bool deleting = true;
                foreach (var size in sizes)
                {
                    if (subDir.EndsWith($"\\{size}"))
                    {
                        deleting = false;
                        break;
                    }
                }
                if (deleting)
                {
                    Directory.Delete(subDir, true);
                }
            }
        }

        public ThumbnailSize? GetSmallestThumbnailSize()
        {
            var sizes = GetThumbnailSizes();
            // исходим из предположения, что обычно бОльшая сторона - это ширина
            return sizes?.OrderBy(x => x.Side).ThenBy(x => x.Pixels).FirstOrDefault();
        }

        /// <summary>
        /// Получить все размеры миниатюр, которые используются в системе в текущий момент
        /// </summary>
        /// <returns></returns>
        private ThumbnailSize[] GetThumbnailSizes()
        {
            var sizes = _settingsService.GetSettings<string>(nameof(Setting.ThumbnailSizes)).Split(',');
            if (sizes.Length == 0)
                return null;
            return sizes.Select(x => new ThumbnailSize(x)).ToArray();
        }

        public void RemoveImage(string imageId)
        {
            // удаляем запись в базе
            _imageGateway.Delete(imageId);
            // удаляем большую картинку
            var path = GetImagePathInternal(imageId, null);
            File.Delete(path);
            // удаляем все миниатюры
            var sizes = GetThumbnailSizes();
            foreach (var thumbnailSize in sizes)
            {
                var thumbPath = GetImagePathInternal(imageId, thumbnailSize);
                if (File.Exists(thumbPath)) // могут быть не все
                {
                    File.Delete(thumbPath);
                }
            }
        }

        public IEnumerable<ImageInfo> GetAllImageInfos()
        {
            string imgDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, IMAGES_DIR);
            var jpgs = Directory.GetFiles(imgDir, "*.jpg", SearchOption.AllDirectories).Concat(Directory.GetFiles(imgDir, "*.jpeg", SearchOption.AllDirectories));
            var gifs = Directory.GetFiles(imgDir, "*.gif", SearchOption.AllDirectories);
            var pngs = Directory.GetFiles(imgDir, "*.png", SearchOption.AllDirectories);
            var files = jpgs.Concat(gifs).Concat(pngs);
            
            var res = files.Select(x =>
            {
                string imageId = x.Split('\\').Last();
                return new ImageInfo()
                {
                    ImageId = imageId,
                    //выравнивание по Москве
                    CreationDate = new FileInfo(x).CreationTimeUtc.ApplySiteTimezone(),
                    Size = new FileInfo(x).Length,
                    AlbumCount = _imageInAlbumGateway.SelectByImage(imageId).Count
                };
            }).OrderByDescending(x => x.CreationDate);
            return res;
        }

        public IEnumerable<ImageInfo> GetGalleryImageInfos()
        {
            return GetAllImageInfos();
        }

        private ImageFormat MimeTypeToImageFormat(string mimeType)
        {
            ImageFormat format;
            switch (mimeType)
            {
                case MIMETYPE_JPG:
                    format = ImageFormat.Jpeg;
                    break;
                case MIMETYPE_GIF:
                    format = ImageFormat.Gif;
                    break;
                case MIMETYPE_PNG:
                    format = ImageFormat.Png;
                    break;
                default:
                    throw new ArgumentException("mimeType");
            }
            return format;
        }

        /// <summary>
        /// Получить путь к изображению или миниатюре
        /// </summary>
        /// <param name="imageId">ID изображения</param>
        /// <param name="size">Минимальный желаемый размер, если нужна миниатюра</param>
        /// <returns></returns>
        private string GetImagePathInternal(string imageId, ThumbnailSize? size)
        {
            string dir = GetDir(imageId, size);

            // в imageId есть расширение
            if (imageId.Contains("."))
            {
                return Path.Combine(dir, imageId);
            }
            // в imageId нет расширения
            string[] exts = {"jpg", "png", "gif", "jpeg"};
            var path = Path.Combine(dir, imageId.ToLower());
            bool found = false;
            foreach (var ext in exts)
            {
                if (File.Exists(path + "." + ext))
                {
                    path = path + "." + ext;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new FileNotFoundException("Image not found");
            }
            return path;
        }

        private string GetDir(string imageId, ThumbnailSize? size)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string first = imageId.Substring(0, 1);
            string second = imageId.Substring(1, 1);
            if (!size.HasValue) // требуется вернуть оригинал
            {
                return Path.Combine(baseDir, IMAGES_DIR, first, second);
            }
            // ищем ближайший размер миниатюры больше заданного
            var nearestSize = GetNearestThumbnailSize(size.Value);
            if (nearestSize == null)
            {
                // все миниатюры меньше, возвращаем папку оригинала картинки
                return Path.Combine(baseDir, IMAGES_DIR, first, second);
            }
            return Path.Combine(baseDir, THUMBNAILS_DIR, nearestSize.Value.SizeString, first, second);
        }

        private ThumbnailSize? GetNearestThumbnailSize(ThumbnailSize size)
        {
            var sizes = GetThumbnailSizes();
            if (sizes.Contains(size))
            {
                return size;
            }
            var sizesOfSelectedSize =
                sizes.Where(x => x.Side == size.Side && x.Pixels > size.Pixels).OrderBy(x => x.Pixels);
            return sizesOfSelectedSize.Any() 
                ? (ThumbnailSize?) sizesOfSelectedSize.FirstOrDefault() 
                : null;
        }

        protected override int CacheExpirationInMinutes => 30;
    }
}
