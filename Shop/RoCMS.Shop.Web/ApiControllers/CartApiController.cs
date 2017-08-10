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

        public CartApiController(ICartService cartService, ISessionValueProviderService sessionService, IShopService shopService, ISecurityService securityService, IShopOrderService shopOrderService)
        {
            _cartService = cartService;
            _sessionService = sessionService;
            _shopService = shopService;
            _securityService = securityService;
            _shopOrderService = shopOrderService;
        }

        [System.Web.Http.HttpGet]
        public Cart GetCart()
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.CartId);
            return _cartService.GetCart(cartId);
        }

        [System.Web.Http.HttpPost]
        public ResultModel AddItem(int goodsId, int count, int? packId)
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.CartId);
            _cartService.AddItemToCart(cartId, goodsId, count, packId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        public ResultModel ChangeItemCount(int goodsId, int count, int? packId)
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.CartId);
            _cartService.UpdatItemCount(cartId, goodsId, count, packId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        public ResultModel RemoveItem(int goodsId, int? packId)
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.CartId);
            _cartService.RemoveItemFromCart(cartId, goodsId, packId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        public ResultModel Clear()
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.CartId);
            _cartService.RemoveCart(cartId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpGet]
        public CartSummary CartSummary()
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.CartId);
            if(cartId.Equals(Guid.Empty))
                return new CartSummary();
            return _cartService.GetCartSummary(cartId);
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateUserDiscount()
        {
            Guid cartId = _sessionService.Get<Guid>(ConstantStrings.CartId);
            _cartService.UpdateUserDiscount(cartId);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        [AjaxValidateAntiForgeryToken]
        public ResultModel ProcessOrder(ProcessOrderRequest req)
        {
            try
            {
                //TODO: перенести в сервис в тракзакцию
                if (req.User != null)
                {
                    var user = req.User;
                    _securityService.RegisterUser(user.Username, user.Password, req.Client.Email);

                    var dbUser = _securityService.GetUser(user.Username);
                    req.Client.UserId = dbUser.UserId;
                }

                Guid cartId = _sessionService.Get<Guid>(ConstantStrings.CartId);
                var cart = _cartService.GetCart(cartId);

                _shopOrderService.ProcessOrder(req.Order, req.Client, cart);
                return ResultModel.Success;
            }
            catch (UserExistsException e)
            {
                return new ResultModel(e);
            }
            catch (Exception)
            {
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
