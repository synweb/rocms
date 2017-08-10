using System;
using System.Collections.Generic;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [System.Web.Http.Authorize]
    [AuthorizeResourcesApi(RoCmsResources.VideoGallery)]
    public class VideoAlbumApiController : ApiController
    {
        private readonly IVideoGalleryService _videoGalleryService;

        public VideoAlbumApiController(IVideoGalleryService videoGalleryService)
        {
            _videoGalleryService = videoGalleryService;
        }

        [HttpPost]
        public ResultModel CreateVideoAlbum([FromBody] string title)
        {
            try
            {
                _videoGalleryService.CreateVideoAlbum(title);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel DeleteVideoAlbum(int albumId)
        {
            try
            {
                _videoGalleryService.RemoveVideoAlbum(albumId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Mvc.HttpPost]
        public ResultModel UpdateSortOrder(int albumId, IList<string> videoIds)
        {
            try
            {
                _videoGalleryService.UpdateVideosSortOrder(albumId, videoIds);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        public class VideoDiffs
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }

        [HttpPost]
        public ResultModel UpdateVideoTitle(string videoId, VideoDiffs diff)
        {
            try
            {
                _videoGalleryService.UpdateVideoTitle(videoId, diff.Title);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel UpdateVideoDescription(string videoId, VideoDiffs diff)
        {
            try
            {
                _videoGalleryService.UpdateVideoDescription(videoId, diff.Description);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel AddVideoToAlbum(int albumId, string videoId)
        {
            try
            {
                _videoGalleryService.AddVideo(albumId, videoId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel DeleteVideo(string videoId)
        {
            try
            {
                _videoGalleryService.RemoveVideo(videoId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [HttpPost]
        public ResultModel UpdateAlbumTitle(int albumId, [FromBody] string title)
        {
            try
            {
                _videoGalleryService.UpdateVideoAlbumTitle(albumId, title);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}