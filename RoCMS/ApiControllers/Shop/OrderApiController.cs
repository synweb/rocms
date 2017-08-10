using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models.Shop;
using RoCMS.Web.Contract.Services;
using RoCMS.Web.Contract.Services.Shop;

namespace RoCMS.ApiControllers.Shop
{
    [AuthorizeResources(RoCmsResources.Shop)]
    public class OrderApiController : ApiController
    {
        private readonly IShopService _shopService;
        private readonly IShopOrderService _shopOrderService;

        public OrderApiController(IShopService shopService, IShopOrderService shopOrderService)
        {
            _shopService = shopService;
            _shopOrderService = shopOrderService;
        }

        [System.Web.Http.HttpGet]
        // GET api/<controller>
        public IEnumerable<Order> GetOrders()
        {
            var res =_shopService.GetOrders().OrderByDescending(x => x.OrderId);
            return res;
        }

        public ResultModel GetOrdersPage(int startIndex, int pageSize)
        {
            int total;
            var orders = _shopOrderService.GetOrderPage(startIndex, pageSize, out total);
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
                _shopService.UpdateOrderState(update.OrderId, update.State);
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
                _shopService.UpdateOrder(order);
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
                _shopService.DeleteOrder(orderId);
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
                int id = _shopService.CreateOrder(order);
                return new ResultModel(true, id);
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }
    }
}