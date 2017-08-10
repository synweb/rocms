using System.Collections.Generic;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IOrderFormService
    {
        int CreateOrderForm(OrderForm form);
        void SaveOrderForm(OrderForm form);
        void DeleteOrderForm(int formId);
        OrderForm GetOrderForm(int formId);

        IList<OrderForm> GetOrderForms();
    }
}