using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using RoCMS.Base.Helpers;
using RoCMS.Data.Gateways;
using RoCMS.Data.Models;
using RoCMS.Web.Contract.Extensions;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;
using Album = RoCMS.Web.Contract.Models.Album;

namespace RoCMS.Web.Services
{
    public class AlbumService: BaseCoreService, IAlbumService
    {
        private readonly IImageService _imageService;
        private readonly ILogService _logService;

        private readonly AlbumGateway _albumGateway = new AlbumGateway();
        private readonly ImageInAlbumGateway  _imageInAlbumGateway = new ImageInAlbumGateway();
        private readonly ImageGateway _imageGateway = new ImageGateway();

        public AlbumService(IImageService imageService, ILogService logService)
        {
            _imageService = imageService;
            _logService = logService;
            CacheExpirationInMinutes = 30;
        }

        #region Images


        public int CreateAlbum(string name, int? ownerId = null)
        {
            var dataRec = new Data.Models.Album() {Name = name, OwnerId = ownerId};
            int id = _albumGateway.Insert(dataRec);
            return id;
        }

        public Album GetAlbum(int albumId)
        {
            var dataRec = _albumGateway.SelectOne(albumId);
            var res = Mapper.Map<Album>(dataRec);

            FillImageCount(res);
            return res;
        }

        public ICollection<Album> GetUserAlbums(int? ownerId = null)
        {
            var dataRecs = _albumGateway.SelectUserAlbums(ownerId);
            var  res =  Mapper.Map<ICollection<Album>>(dataRecs);
            foreach (var album in res)
            {
                FillImageCount(album);
            }
            return res;
        }

        public ICollection<string> GetAlbumImageIds(int albumId)
        {
            // для ускорения можно создать хранимку, которая будет цеплять только ImageId
            var iias = _imageInAlbumGateway.SelectByAlbum(albumId);
            return iias.Select(x => x.ImageId).ToList();
        }

        public void RemoveImageFromAlbum(int albumId, string imageId)
        {
            _imageInAlbumGateway.Delete(albumId, imageId);
        }

        public void RemoveAlbum(int albumId)
        {
            _albumGateway.Delete(albumId);
        }

        public string GetRandomImageFromAlbum(int albumId)
        {
            var images = GetAlbumImageIds(albumId).ToArray();
            if (!images.Any())
            {
                throw new ArgumentException("В альбоме нет фотографий");
            }
            int index = RandomHelper.GetRandom(images.Length);
            return images[index];
        }

        public ICollection<Album> GetAlbums()
        {
            var dataRecs = _albumGateway.Select();
            var res = Mapper.Map<ICollection<Album>>(dataRecs);
            foreach (var album in res)
            {
                FillImageCount(album);
            }
            return res;
        }

        public ICollection<Album> GetSystemAlbums()
        {
            // для ускорения можно забрать данные отдельной хранимкой
            var dataRecs = _albumGateway.Select().Where(x => !x.OwnerId.HasValue);
            var res = Mapper.Map<ICollection<Album>>(dataRecs);
            foreach (var album in res)
            {
                FillImageCount(album);
            }
            return res;
        }

        private void FillImageCount(Album album)
        {
            album.ImageCount = _imageInAlbumGateway.SelectCountByAlbum(album.AlbumId);
        }

        public void AddImageToAlbum(int albumId, string imageId)
        {
            var title = _imageGateway.SelectOne(imageId).InitialFilename;
            _imageInAlbumGateway.Insert(new ImageInAlbum()
            {
                AlbumId = albumId,
                ImageId = imageId,
                Title = title
            });
            var dataAlbum = _albumGateway.SelectOne(albumId);
            if (!string.IsNullOrEmpty(dataAlbum.WatermarkImageId))
            {
                _imageService.ApplyWatermark(imageId, dataAlbum.WatermarkImageId);
            }
        }

        public IEnumerable<AlbumImageInfo> GetAlbumImages(int albumId)
        {
            var images = _imageInAlbumGateway.SelectByAlbum(albumId).ToList();
            images.Sort((x, y) => x.SortOrder.CompareTo(y.SortOrder));
            var result = images.Select(img =>
            {
                try
                {
                    var path = _imageService.GetImagePath(img.ImageId, null, false);
                    var creationDate = new FileInfo(path).CreationTimeUtc.ApplySiteTimezone();
                    return new AlbumImageInfo()
                    {
                        //выравнивание по Москве
                        CreationDate = creationDate,
                        Description = img.Description,
                        Title = img.Title,
                        ImageId = img.ImageId,
                        Size = new FileInfo(path).Length,
                        DestinationUrl = img.DestinationUrl
                    };
                }
                catch (Exception e)
                {
                    _logService.TraceMessage($"Image unavailable (ID:{img.ImageId})");
                    return new AlbumImageInfo()
                    {
                        ImageId = img.ImageId,
                        Description = img.Description,
                        Title = img.Title,
                        DestinationUrl = img.DestinationUrl
                    };
                }
            }).ToList();
            return result;
        }

        public void UpdateImageDescription(int albumId, string imageId, string description)
        {
            var iia = _imageInAlbumGateway.SelectOne(albumId, imageId);
            iia.Description = description;
            _imageInAlbumGateway.Update(iia);
        }

        public void UpdateImagesSortOrder(int albumId, IList<string> imageIds)
        {
            int order = 0;
            foreach (var imageId in imageIds)
            {
                var img = _imageInAlbumGateway.SelectOne(albumId, imageId);
                img.SortOrder = order++;
                _imageInAlbumGateway.Update(img);
            }
        }

        public void UpdateAlbum(Album album)
        {
            var dataRec = Mapper.Map<Data.Models.Album>(album);
            _albumGateway.Update(dataRec);
        }
        
        public void UpdateImageDestinationUrl(int albumId, string imageId, string destinationUrl)
        {
            var iia = _imageInAlbumGateway.SelectOne(albumId, imageId);
            iia.DestinationUrl = destinationUrl;
            _imageInAlbumGateway.Update(iia);
        }

        public void SetAlbumWatermark(int albumId, string watermarkImageId)
        {
            if (string.IsNullOrEmpty(watermarkImageId))
            {
                // удаляем вотермарку, восстанавливаем картинки
                foreach (var imageId in GetAlbumImageIds(albumId))
                {
                    _imageService.RestoreImage(imageId);
                }
            }
            else
            {
                // ставим вотермарку, бэкапим картинки
                foreach (var imageId in GetAlbumImageIds(albumId))
                {
                    _imageService.ApplyWatermark(imageId, watermarkImageId);
                }

            }

            var dataAlbum = _albumGateway.SelectOne(albumId);
            dataAlbum.WatermarkImageId = watermarkImageId;
            _albumGateway.Update(dataAlbum);
        }

        public void UpdateImageTitle(int albumId, string imageId, string title)
        {
            var iia = _imageInAlbumGateway.SelectOne(albumId, imageId);
            iia.Title = title;
            _imageInAlbumGateway.Update(iia);
        }

        #endregion
        
        protected override int CacheExpirationInMinutes { get; }
    }
}
