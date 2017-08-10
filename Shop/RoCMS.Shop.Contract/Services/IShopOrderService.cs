using System.Collections.Generic;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopOrderService
    {
        IEnumerable<Order> GetOrderPage(int startIndex, int pageSize, out int total, int? clientId = null);
        Order GetOrder(int orderId);
        int CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int orderId);
        IList<Order> GetOrders();
        void ProcessOrder(Order order, Client client, Cart cart);
        void UpdateOrderState(int orderId, OrderState state);
    }
}
