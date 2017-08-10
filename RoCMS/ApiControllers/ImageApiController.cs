using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{

    [System.Web.Http.Authorize]
    [AuthorizeResourcesApi(RoCmsResources.Gallery)]
    public class ImageApiController: ApiController
    {
        private readonly IImageService _imageService;

        public ImageApiController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [System.Web.Http.HttpPost]
        public ResultModel RemoveImage(string id)
        {
            bool succeed = true;
                _imageService.RemoveImage(id);
            try
            {
                //Изображение удаляется из всех альбомов и таблицы с изображениями
            }
            catch
            {
                succeed = false;
            }
            return new ResultModel(succeed);
        }
    }
}