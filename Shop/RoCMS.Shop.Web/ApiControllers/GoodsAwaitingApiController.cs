using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    public class GoodsAwaitingApiController : ApiController
    {
        private readonly IShopGoodsAwaitingService _shopGoodsAwaitingService;
        private readonly IPrincipalResolver _principalResolver;
        public GoodsAwaitingApiController(IShopGoodsAwaitingService shopGoodsAwaitingService, IPrincipalResolver principalResolver)
        {
            _shopGoodsAwaitingService = shopGoodsAwaitingService;
            _principalResolver = principalResolver;
        }

        public ResultModel Create(CreateAwaitingModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Email) && String.IsNullOrEmpty(model.Phone))
                {
                    return new ResultModel(false, "Укажите телефон или email");
                }

                if (!String.IsNullOrEmpty(model.Email))
                {
                    if (ValidationHelper.ValidateEmail(model.Email))
                    {
                        GoodsAwaiting ga = new GoodsAwaiting()
                        {
                            UserId = _principalResolver.GetUserIdIfAuthenticated(),
                            Contact = model.Email,
                            ContactType = ContactType.Email,
                            GoodsId = model.GoodsId
                        };
                        _shopGoodsAwaitingService.CreateGoodsAwaiting(ga);
                    }
                    else
                    {
                        return new ResultModel(false, "Неправильно введен email");
                    }
                }
                if (!String.IsNullOrEmpty(model.Phone))
                {
                    if (ValidationHelper.ValidatePhone(model.Phone))
                    {
                        GoodsAwaiting ga = new GoodsAwaiting()
                        {
                            UserId = _principalResolver.GetUserIdIfAuthenticated(),
                            Contact = model.Phone,
                            ContactType = ContactType.Phone,
                            GoodsId = model.GoodsId
                        };
                        _shopGoodsAwaitingService.CreateGoodsAwaiting(ga);
                    }
                    else
                    {
                        return new ResultModel(false, "Неправильно введен телефон");
                    }
                }
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return ResultModel.Error;
            }

        }

        public class CreateAwaitingModel
        {
            public int GoodsId { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }
}
