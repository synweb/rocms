using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using AutoMapper;
using RoCMS.Data.Gateways;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Shop.Data.Gateways;
using RoCMS.Shop.Data.Models;
using RoCMS.Web.Contract.Services;
using Client = RoCMS.Shop.Contract.Models.Client;
using Order = RoCMS.Shop.Contract.Models.Order;
using OrderState = RoCMS.Shop.Contract.Models.OrderState;

namespace RoCMS.Shop.Services
{
    class ShopClientService: BaseShopService, IShopClientService
    {
        private readonly ClientGateway _clientGateway = new ClientGateway();
        //private readonly UserGateway _userGateway = new UserGateway();
        private readonly RegularCustomerDiscountGateway _regularCustomerDiscountGateway = new RegularCustomerDiscountGateway();
        private readonly ISecurityService _securityService;

        public ShopClientService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public Client GetClientByUserId(int userId)
        {
            var dataRes = _clientGateway.SelectOneByUserId(userId);
            var res = Mapper.Map<Client>(dataRes);
            return res;
        }

        public Client GetClient(int clientId)
        {
            var dataRes = _clientGateway.SelectOne(clientId);
            var res = Mapper.Map<Client>(dataRes);
            return res;
        }

        public int CreateClient(Client client)
        {
            var dataRec = Mapper.Map<Data.Models.Client>(client);
            int id = _clientGateway.Insert(dataRec);
            return id;
        }

        public void UpdateClient(Client client)
        {
            var dataRec = Mapper.Map<Data.Models.Client>(client);
            _clientGateway.Update(dataRec);
        }

        public void DeleteClient(int clientId)
        {
            _clientGateway.Delete(clientId);
        }

        public IList<Client> GetClients()
        {
            var dataRes = _clientGateway.Select();
            var res = Mapper.Map<IList<Client>>(dataRes);
            return res;
        }

        //TODO: сделать отдельный ShopOrderService, перенести туда всю логику, связанную с заказами
        public IEnumerable<Order> GetOrdersByUserId(int userId)
        {
            //TODO:
            //throw new NotImplementedException();
            return new List<Order>();
        }

        public int GetRegularDiscountForClient(int userId)
        {
            var client = _clientGateway.SelectOneByUserId(userId);
            if (client == null) return 0;

            var res = _regularCustomerDiscountGateway.Select().OrderByDescending(x => x.MinimalSum);
            if (!res.Any()) return 0;

            int total;
            var shopOrderService = DependencyResolver.Current.GetService<IShopOrderService>();
            IEnumerable<Order> orders = shopOrderService.GetOrderPage(1, Int32.MaxValue, out total, client.ClientId).Where(x => x.State == OrderState.Completed);


            decimal totalSum = client.InitialAmount + orders.Sum(x => x.GoodsInOrder.Sum(y => ((y.Price - y.Price * (decimal)x.TotalDiscount / 100m) * (decimal)y.Quantity)));
            foreach (var discount in res)
            {
                if (totalSum >= discount.MinimalSum)
                {
                    return discount.Discount;
                }
            }

            return 0;
        }

        public IList<RegularClientDiscount> GetRegularClientDiscounts()
        {
            var dataRes = _regularCustomerDiscountGateway.Select();
            var res = Mapper.Map<IList<RegularClientDiscount>>(dataRes);
            return res;
        }

        public int CreateRegularClientDiscounts(RegularClientDiscount discount)
        {
            var dataRec = Mapper.Map<RegularCustomerDiscount>(discount);
            int id = _regularCustomerDiscountGateway.Insert(dataRec);
            return id;
        }

        public void UpdateRegularClientDiscount(RegularClientDiscount discount)
        {
            var dataRec = Mapper.Map<RegularCustomerDiscount>(discount);
            _regularCustomerDiscountGateway.Update(dataRec);
        }

        public void DeleteRegularClientDiscount(int id)
        {
            _regularCustomerDiscountGateway.Delete(id);
        }

        public IEnumerable<Client> GetClientsPage(int startIndex, int pageSize, out int total)
        {
            var dataRes = _clientGateway.SelectPage(startIndex, pageSize, out total);
            var res = Mapper.Map<IEnumerable<Client>>(dataRes);
            return res;
        }

        public void UpdateClientEmail(int clientId, string email)
        {
            var client = _clientGateway.SelectOne(clientId);
            if (client.UserId == null)
                return;
            var user = _securityService.GetUser(client.UserId.Value);
            using (var ts = new TransactionScope())
            {
                user.Email = email;
                _securityService.UpdateUser(user);
                client.Email = email;
                _clientGateway.Update(client);
                ts.Complete();
            }
        }
    }
}
