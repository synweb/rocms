using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    public class FavouriteItemApiController : ApiController
    {
        private readonly IFavouriteItemsService _favouriteItemsService;
        private ISessionValueProviderService _sessionService;
        private ILogService _logService;

        public FavouriteItemApiController(IFavouriteItemsService favouriteItemsService, ISessionValueProviderService sessionService, ILogService logService)
        {
            _favouriteItemsService = favouriteItemsService;
            _sessionService = sessionService;
            _logService = logService;
        }

        [HttpPost]
        public ResultModel Add(int heartId)
        {
            try
            {
                Guid sessionId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
                _favouriteItemsService.Add(sessionId, heartId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return ResultModel.Error;
            }
        }

        [HttpPost]
        public ResultModel Delete(int heartId)
        {
            try
            {
                Guid sessionId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
                _favouriteItemsService.Delete(sessionId, heartId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return ResultModel.Error;
            }
        }

        [HttpGet]
        public ResultModel HasItems()
        {
            try
            {
                Guid sessionId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
                bool res = _favouriteItemsService.GetFavouriteItems(sessionId).Any();
                return new ResultModel(true, new { hasItems = res });
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return ResultModel.Error;
            }
        }
    }
}
