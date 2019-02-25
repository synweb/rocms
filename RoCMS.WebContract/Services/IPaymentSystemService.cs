using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Services
{
    public interface IPaymentSystemService
    {
        /// <summary>
        /// Возвращает ссылку на форму оплаты для редиректа
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderType"></param>
        /// <param name="amount"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        string ProcessPayment(Guid orderId, decimal amount, string returnUrl);
    }
}
