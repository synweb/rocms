﻿using System.Web.Http;
using RoCMS.Base.Models;
using RoCMS.Shop.Contract.Models;
using RoCMS.Shop.Contract.Services;

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
                return new ResultModel(true, client);
            }
            return ResultModel.Error;
        }

        [System.Web.Http.HttpGet]
        public ResultModel GetClientsPage(int startIndex, int pageSize)
        {
            int total;
            var clients = _clientService.GetClientsPage(startIndex, pageSize, out total);
            var res = new {Total = total, Clients = clients};
            return new ResultModel(true, res);
        }

        [System.Web.Http.HttpGet]
        public ResultModel GetForUser(int userId)
        {
            var client = _clientService.GetClientByUserId(userId);
            if (client != null)
            {
                return new ResultModel(true, client);
            }
            return ResultModel.Error;
        }

        [System.Web.Http.HttpPost]
        public ResultModel Update(Client client)
        {
            _clientService.UpdateClient(client);
            return ResultModel.Success;
        }

        [System.Web.Http.HttpPost]
        public ResultModel DeleteClient(int clientId)
        {
            _clientService.DeleteClient(clientId);
            return ResultModel.Success;
        }
    }
}
