using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Models.Email;
using RoCMS.Shop.Contract.Models.Exceptions;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Web.Contract.Extensions;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Shop.Services
{
    class ShopOrderService: BaseShopService, IShopOrderService
    {
        private readonly IRazorEngineService _razorService;
        private readonly IShopClientService _clientService;
        private readonly IMailService _mailService;
        private readonly ISettingsService _settingsService;
        private readonly IShopService _shopService;
        private readonly IShopPickupPointService _pickupPointService;
        private readonly OrderGateway _orderGateway = new OrderGateway();
        private readonly GoodsInOrderGateway _goodsInOrderGateway = new GoodsInOrderGateway();
        public ShopOrderService(IRazorEngineService razorService, IShopClientService clientService, IMailService mailService, ISettingsService settingsService, IShopService shopService, IShopPickupPointService pickupPointService)
        {
            _razorService = razorService;
            _clientService = clientService;
            _mailService = mailService;
            _settingsService = settingsService;
            _shopService = shopService;
            _pickupPointService = pickupPointService;
        }
        public IEnumerable<Order> GetOrderPage(int startIndex, int pageSize, out int total, int? clientId = null)
        {
            var dataRes = _orderGateway.SelectPage(startIndex, pageSize, out total, clientId);
            var res = Mapper.Map<IList<Order>>(dataRes);
            foreach (var order in res)
            {
                FillOrderGoods(order);
                FillClient(order);
            }
            return res;
        }

        public Order GetOrder(int orderId)
        {
            var dataRes = _orderGateway.SelectOne(orderId);
            var res = Mapper.Map<Order>(dataRes);
            FillOrderGoods(res);
            FillClient(res);
            FillPickUpPoint(res);
            return res;
        }

        private void FillPickUpPoint(Order res)
        {
            try
            {
                if (res.PickUpPointId.HasValue)
                {
                    res.PickUpPoint = _pickupPointService.GetPickupPoint(res.PickUpPointId.Value);
                }
            }
            catch
            {
                
            }
        }

        private void FillClient(Order order)
        {
            order.Client = _clientService.GetClient(order.ClientId);
        }

        private void FillOrderGoods(Order order)
        {
            var dataGios = _goodsInOrderGateway.SelectByOrder(order.OrderId);
            var gios = Mapper.Map<List<GoodsInOrder>>(dataGios);
            foreach (var goodsInOrder in gios)
            {
                try
                {
                    goodsInOrder.Goods = _shopService.GetGoods(goodsInOrder.HeartId);
                }
                catch (GoodsNotFoundException)
                {
                    //TODO: заплатка на скорую руку. Разрулить
                    GoodsItemGateway gateway = new GoodsItemGateway();
                    var goods = gateway.SelectOne(goodsInOrder.HeartId);
                    if (goods != null)
                    {
                        goodsInOrder.Goods = Mapper.Map<GoodsItem>(goods);
                    }
                    else
                    {
                        goodsInOrder.Goods = new GoodsItem() { Name = "Товар более не доступен"};
                    }
                    
                }
            }
            order.GoodsInOrder = gios;
        }

        public int CreateOrder(Order order)
        {
            var dataRec = Mapper.Map<Data.Models.Order>(order);
            using (var ts = new TransactionScope())
            {
                int id = _orderGateway.Insert(dataRec);
                var gios = Mapper.Map<ICollection<Data.Models.GoodsInOrder>>(order.GoodsInOrder);
                foreach (var goodsInOrder in gios)
                {
                    goodsInOrder.OrderId = id;
                    _goodsInOrderGateway.Insert(goodsInOrder);
                }
                ts.Complete();
                return id;
            }
        }

        public void UpdateOrder(Order order)
        {
            var dataRec = Mapper.Map<Data.Models.Order>(order);
            _orderGateway.Update(dataRec);
            // данные о товарах не меняем
        }

        public void DeleteOrder(int orderId)
        {
            _orderGateway.Delete(orderId);
        }

        public IList<Order> GetOrders()
        {
            var dataRes = _orderGateway.Select();
            var res = Mapper.Map<IList<Order>>(dataRes);
            foreach (var order in res)
            {
                FillOrderGoods(order);
                FillClient(order);
            }
            return res;
        }

        public void ProcessOrder(Order order, Client client, Cart cart)
        {
            order.GoodsInOrder = Mapper.Map<List<GoodsInOrder>>(cart.CartItems);
            if (client.UserId.HasValue)
            {
                order.TotalDiscount = _clientService.GetRegularDiscountForClient(client.UserId.Value);
            }
            order.State = OrderState.New;
            using (var ts = new TransactionScope())
            {
                if (client.ClientId == 0)
                {
                    int clientId = _clientService.CreateClient(client);
                    order.ClientId = clientId;
                }
                else
                {
                    order.ClientId = client.ClientId;
                }
                order.OrderId = CreateOrder(order);
                ts.Complete();
            }

            //Перезагружаем заказ, чтобы получить его со всеми заполненными полями
            order = GetOrder(order.OrderId);

            SendOrderToInternalEmail(order, client);
            SendOrderToClientEmail(order, client);
        }
        private void SendOrderToInternalEmail(Order order,
    Client client)
        {
            string htmlString = _razorService.RenderEmailMessage("ShopOrderAdmin", new ShopOrderEmail { Order = order, Client = client });
            var msg = new MailMsg()
            {
                Receiver = _settingsService.GetSettings<string>("OrderEmailAddress"),
                Body = htmlString,
                Subject = "Заказ в интернет-магазине: " + DateTime.UtcNow.ApplySiteTimezone()
            };
            _mailService.Send(msg);
        }

        private void SendOrderToClientEmail(Order order,
            Client client)
        {
            string htmlString = _razorService.RenderEmailMessage("ShopOrderClient", new ShopOrderEmail { Order = order, Client = client });
            var msg = new MailMsg()
            {
                Receiver = client.Email,
                Body = htmlString,
                Subject = "Заказ в интернет-магазине: " + DateTime.UtcNow.ApplySiteTimezone()
            };
            _mailService.Send(msg);
        }

        public void UpdateOrderState(int orderId, OrderState state)
        {
            var order = _orderGateway.SelectOne(orderId);
            order.State = Mapper.Map<Data.Models.OrderState>(state);
            _orderGateway.Update(order);
        }


    }
}
