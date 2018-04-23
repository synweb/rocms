using System;
using System.Web.Http;
using RoCMS.Base.Exceptions;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    public class CartApiController : ApiController
    {
        private ICartService _cartService;
        private IShopService _shopService;
        private ISessionValueProviderService _sessionService;
        private ISecurityService _securityService;
        private readonly IShopOrderService _shopOrderService;
        private readonly ILogService _logService;

        public CartApiController(ICartService cartService, ISessionValueProviderService sessionService, IShopService shopService, ISecurityService securityService, IShopOrderService shopOrderService, ILogService logService)
        {
            _cartService = cartService;
            _sessionService = sessionService;
            _shopService = shopService;
            _securityService = securityService;
            _shopOrderService = shopOrderService;
            _logService = logService;
        }

        [System.Web.Http.HttpGet]
        public Cart GetCart()
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
            return _cartService.GetCart(cartId);
        }

        [System.Web.Http.HttpPost]
        public ResultModel AddItem(int heartId, int count, int? packId)
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
            _cartService.AddItemToCart(cartId, heartId, count, packId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        public ResultModel ChangeItemCount(int heartId, int count, int? packId)
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
            _cartService.UpdatItemCount(cartId, heartId, count, packId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        public ResultModel RemoveItem(int heartId, int? packId)
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
            _cartService.RemoveItemFromCart(cartId, heartId, packId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        public ResultModel Clear()
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
            _cartService.RemoveCart(cartId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpGet]
        public CartSummary CartSummary()
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
            if(cartId.Equals(Guid.Empty))
                return new CartSummary();
            return _cartService.GetCartSummary(cartId);
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateUserDiscount()
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
            _cartService.UpdateUserDiscount(cartId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        [AjaxValidateAntiForgeryToken]
        public ResultModel ProcessOrder(ProcessOrderRequest req)
        {
            try
            {
                if (req.User != null)
                {
                    var user = req.User;
                    _securityService.RegisterUser(user.Username, user.Password, req.Client.Email);

                    var dbUser = _securityService.GetUser(user.Username);
                    req.Client.UserId = dbUser.UserId;
                }

                Guid cartId = _sessionService.Get<Guid>(ConstantStrings.SessionId);
                var cart = _cartService.GetCart(cartId);

                _shopOrderService.ProcessOrder(req.Order, req.Client, cart);
                return ResultModel.Success;
            }
            catch (UserExistsException e)
            {
                _logService.LogError(e);
                return new ResultModel(e);
            }
            catch (Exception e)
            {
                _logService.LogError(e);
                return new ResultModel(false, "При формировании заказа произошла ошибка");
            }
        }

        public class ProcessOrderRequest
        {
            public Order Order { get; set; }
            public Client Client { get; set; }
            public Cart Cart { get; set; }
            public UserRegistration User { get; set; }
        }
    }
}
