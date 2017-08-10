using System.Collections.Generic;
using RoCMS.Shop.Contract.Models;

namespace RoCMS.Shop.Contract.Services
{
    public interface IShopClientService
    {
        Client GetClientByUserId(int userId);

        Client GetClient(int clientId);
        int CreateClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(int clientId);
        IList<Client> GetClients();
        IEnumerable<Order> GetOrdersByUserId(int userId);
        int GetRegularDiscountForClient(int userId);
        IList<RegularClientDiscount> GetRegularClientDiscounts();
        int CreateRegularClientDiscounts(RegularClientDiscount discount);
        void UpdateRegularClientDiscount(RegularClientDiscount discount);
        void DeleteRegularClientDiscount(int id);
        IEnumerable<Client> GetClientsPage(int startIndex, int pageSize, out int total);
        void UpdateClientEmail(int clientId, string email);
    }
}
