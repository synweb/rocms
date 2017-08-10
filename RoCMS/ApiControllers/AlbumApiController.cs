using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [System.Web.Http.Authorize]
    [AuthorizeResourcesApi(RoCmsResources.Albums)]
    public class AlbumApiController : ApiController
    {
        private readonly IAlbumService _albumService;
        private IImageService _imageService;

        public AlbumApiController(IAlbumService albumService, IImageService imageService)
        {
            _albumService = albumService;
            _imageService = imageService;
        }

        [System.Web.Http.HttpPost]
        public ResultModel AddImageToAlbum(int albumId, string imageId)
        {
            try
            {
                _albumService.AddImageToAlbum(albumId, imageId);
                return new ResultModel(true);
            }
            catch(Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateSortOrder(int albumId, IList<string> imageIds)
        {
            try
            {
                _albumService.UpdateImagesSortOrder(albumId, imageIds);
                return ResultModel.Success;
            }
            catch(Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateImageTitle(int albumId, string imageId, AlbumImageInfo image)
        {
            try
            {
                _albumService.UpdateImageTitle(albumId, imageId, image.Title);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateImageDescription(int albumId, string imageId, AlbumImageInfo image)
        {
            try
            {
                _albumService.UpdateImageDescription(albumId, imageId, image.Description);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateImageDestinationUrl(int albumId, string imageId, AlbumImageInfo image)
        {
            try
            {
                _albumService.UpdateImageDestinationUrl(albumId, imageId, image.DestinationUrl);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateAlbum(Album album)
        {
            try
            {
                _albumService.UpdateAlbum(album);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
        
        [System.Web.Http.HttpPost]
        public ResultModel RemoveImageFromAlbum(int albumId, string id)
        {
            try
            {
                //Изображение удаляется и из альбома, и из базы
                //_imageService.RemoveImage(id);
                _albumService.RemoveImageFromAlbum(albumId, id);
                var imageInfo = _imageService.GetImageInfo(id);
                if(imageInfo.AlbumCount == 0)
                {
                    _imageService.RemoveImage(id);
                }
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return ResultModel.Error;
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel CreateAlbum([FromBody] string title)
        {
            try
            {
                int id = _albumService.CreateAlbum(title, null);
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel DeleteAlbum(int albumId)
        {
            try
            {
                _albumService.RemoveAlbum(albumId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}
