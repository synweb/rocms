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

        public UserAlbumApiController(IAlbumService albumService, ISecurityService securityService)
        {
            _albumService = albumService;
            _securityService = securityService;
        }
        
        public ResultModel GetUserAlbumId()
        {
            try
            {
                return new ResultModel(true, GetAlbumId());
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
        public ResultModel AddImageToUserAlbum(string imageId)
        {
            try
            {
                int albumId = GetAlbumId();
                _albumService.AddImageToAlbum(albumId, imageId);
                return new ResultModel(true);
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        private int GetAlbumId()
        {
            var album = _albumService.GetUserAlbums(UserId).FirstOrDefault();
            if (album != null) 
                return album.AlbumId;
            string username = _securityService.GetUser(UserId).Username;
            string albumName = string.Format("Альбом пользователя {0} ({1})", username, UserId);
            int id = _albumService.CreateAlbum(albumName, UserId);
            return id;
        }


        private int UserId
        {
            get
            {
                return AuthenticationHelper.GetInstance().GetUserId(HttpContext.Current);
            }
        }

    }
}