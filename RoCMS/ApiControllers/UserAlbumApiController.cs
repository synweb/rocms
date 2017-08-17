using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [System.Web.Http.Authorize]
    public class UserAlbumApiController: ApiController
    {
        private readonly IAlbumService _albumService;
        private readonly ISecurityService _securityService;
        private readonly ILogService _logService;

        public UserAlbumApiController(IAlbumService albumService, ISecurityService securityService, ILogService logService)
        {
            _albumService = albumService;
            _securityService = securityService;
            _logService = logService;
        }
        
        public ResultModel GetUserAlbumId()
        {
            try
            {
                return new ResultModel(true, GetAlbumId());
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
        public ResultModel AddImageToUserAlbum(string imageId)
        {
            try
            {
                int albumId = GetAlbumId();
                _albumService.AddImageToAlbum(albumId, imageId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }

        private int GetAlbumId()
        {
            int userId = AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
            var album = _albumService.GetUserAlbums(userId).FirstOrDefault();
            if (album != null) 
                return album.AlbumId;
            string username = _securityService.GetUser(userId).Username;
            string albumName = string.Format("Альбом пользователя {0} ({1})", username, userId);
            int id = _albumService.CreateAlbum(albumName, userId);
            return id;
        }
        

    }
}