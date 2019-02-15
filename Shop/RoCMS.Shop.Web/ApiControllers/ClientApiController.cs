using System.Threading;
using System.Web;
using System.Web.Http;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Helpers;
using RoCMS.Shop.Contract;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;
using RoCMS.Web.Contract.Models.Security;

namespace RoCMS.Shop.Web.ApiControllers
{
    [System.Web.Http.Authorize]
    public class ClientApiController : ApiController
    {
        private IShopClientService _clientService;


        public ClientApiController(IShopClientService clientService)
        {
            _clientService = clientService;
        }

        [System.Web.Http.HttpGet]
        public ResultModel Get(int clientId)
        {
            var client = _clientService.GetClient(clientId);

            if (client != null)
            {

                RoPrincipal currentPrincipal = Thread.CurrentPrincipal as RoPrincipal;
                if (currentPrincipal == null || currentPrincipal.UserId != client.UserId && !currentPrincipal.IsAuthorizedForResource(ShopRoCmsResources.Shop))
                {
                    return ResultModel.Error;
                }


                return new ResultModel(true, client);
            }

            return ResultModel.Error;
        }

        [AuthorizeResourcesApi(ShopRoCmsResources.Shop)]
        [System.Web.Http.HttpGet]
        public ResultModel GetClientsPage(int startIndex, int pageSize)
        {
            int total;
            var clients = _clientService.GetClientsPage(startIndex, pageSize, out total);
            var res = new { Total = total, Clients = clients };
            return new ResultModel(true, res);
        }

        [System.Web.Http.HttpGet]
        public ResultModel GetForUser(int userId)
        {

            RoPrincipal currentPrincipal = Thread.CurrentPrincipal as RoPrincipal;
            if (currentPrincipal == null || currentPrincipal.UserId != userId && !currentPrincipal.IsAuthorizedForResource(ShopRoCmsResources.Shop))
            {
                return ResultModel.Error;
            }

            var client = _clientService.GetClientByUserId(userId);
            if (client != null)
            {
                return new ResultModel(true, client);
            }
            return ResultModel.Error;
        }

        [AuthorizeResourcesApi(ShopRoCmsResources.Shop)]
        [System.Web.Http.HttpPost]
        public ResultModel Update(Client client)
        {
            _clientService.UpdateClient(client);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        public ResultModel UpdateInfo(Client client)
        {
            var updatingClient = _clientService.GetClient(client.ClientId);
            RoPrincipal currentPrincipal = Thread.CurrentPrincipal as RoPrincipal;
            if (currentPrincipal == null || currentPrincipal.UserId != updatingClient.UserId && !currentPrincipal.IsAuthorizedForResource(ShopRoCmsResources.Shop))
            {
                return ResultModel.Error;
            }

            _clientService.UpdateClientInfo(client);
            return ResultModel.Success;
        }

        [AuthorizeResourcesApi(ShopRoCmsResources.Shop)]
        [System.Web.Http.HttpPost]
        public ResultModel DeleteClient(int clientId)
        {
            _clientService.DeleteClient(clientId);
            return ResultModel.Success;
        }
    }
}
