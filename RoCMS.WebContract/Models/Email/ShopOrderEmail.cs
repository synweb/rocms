using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models.Shop;

namespace RoCMS.Web.Contract.Models.Email
{
    public class ShopOrderEmail
    {
        public Order Order { get; set; }
        public Client Client { get; set; }
    }
}
