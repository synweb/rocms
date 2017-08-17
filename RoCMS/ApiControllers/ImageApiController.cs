using System;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    [AuthorizeResourcesApi(RoCmsResources.Gallery)]
    public class ImageApiController: ApiController
    {
        private readonly IImageService _imageService;
        private readonly ILogService _logService;

        public ImageApiController(IImageService imageService, ILogService logService)
        {
            _imageService = imageService;
            _logService = logService;
        }

        [HttpPost]
        public ResultModel RemoveImage(string id)
        {
            try
            {
                _imageService.RemoveImage(id);
                // Изображение удаляется из таблицы с изображениями и каскадно из всех альбомов 
                return ResultModel.Success;
            }
            catch(Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
        }
    }
}