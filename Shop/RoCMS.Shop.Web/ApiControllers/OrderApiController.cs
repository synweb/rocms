using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

namespace RoCMS.Shop.Web.ApiControllers
{
    [AuthorizeResourcesApi(ShopRoCmsResources.Shop)]
    public class OrderApiController : ApiController
    {
        private readonly IShopOrderService _shopOrderService;

        public OrderApiController(IShopOrderService shopOrderService)
        {
            _shopOrderService = shopOrderService;
        }

        [System.Web.Http.HttpGet]
        // GET api/<controller>
        public IEnumerable<Order> GetOrders()
        {
            var res = _shopOrderService.GetOrders().OrderByDescending(x => x.OrderId);
            return res;
        }

        public ResultModel GetOrdersPage(int startIndex, int pageSize, int? clientId)
        {
            int total;
            var orders = _shopOrderService.GetOrderPage(startIndex, pageSize, out total, clientId);
            var res = new
            {
                Total = total,
                Orders = orders
            };
            return new ResultModel(true, res);
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateState(OrderStateUpdate update)
        {
            try
            {
                _shopOrderService.UpdateOrderState(update.OrderId, update.State);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        public class OrderStateUpdate
        {
            public int OrderId { get; set; }
            public OrderState State { get; set; }
        }

        [System.Web.Http.HttpPost]
        public ResultModel Update(Order order)
        {
            try
            {
                _shopOrderService.UpdateOrder(order);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel Delete(int orderId)
        {
            try
            {
                _shopOrderService.DeleteOrder(orderId);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [System.Web.Http.HttpPost]
        public ResultModel Create(Order order)
        {
            try
            {
                int id = _shopOrderService.CreateOrder(order);
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}