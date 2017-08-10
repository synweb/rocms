using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using RoCMS.Base;
using RoCMS.Base.ForWeb.Models.Filters;
using RoCMS.Base.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Security;
using RoCMS.Web.Contract.Services;

namespace RoCMS.ApiControllers
{
    public class UserApiController : ApiController
    {
        private readonly ISecurityService _securityService;
        private readonly IPasswordTicketService _passwordTicketService;
        private readonly IMailService _mailService;
        private readonly ISettingsService _settingsService;
        private readonly IPrincipalResolver _principalResolver;

        public UserApiController(IPasswordTicketService passwordTicketService, IMailService mailService, ISecurityService securityService, ISettingsService settingsService, IPrincipalResolver principalResolver)
        {
            _passwordTicketService = passwordTicketService;
            _mailService = mailService;
            _securityService = securityService;
            _settingsService = settingsService;
            _principalResolver = principalResolver;
        }

        [System.Web.Http.HttpGet]
        public User GetCurrentUserInfo()
        {
            RoPrincipal currentPrincipal = Thread.CurrentPrincipal as RoPrincipal;
            if (currentPrincipal == null) return null;

            return new User() { UserId = currentPrincipal.UserId, Username = User.Identity.Name };
        }

        [AuthorizeResourcesApi(RoCmsResources.Users)]
        [System.Web.Http.HttpGet]
        public ResultModel GetUsers()
        {
            var res = _securityService.GetUsers();

            foreach (var user in res)
            {
                user.Password = "***";
            }
            return new ResultModel(true, res);
        }

        [System.Web.Http.HttpPost]
        public ResultModel CreateTicket(string email)
        {
            try
            {
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                if (!Regex.IsMatch(email, pattern))
                {
                    return ResultModel.Error;
                }
                var token = _passwordTicketService.CreateTicket(email);
                return ResultModel.Success;
            }
            catch (NullReferenceException e)
            {
                return new ResultModel(false, "Пользователь с таким Email не зарегистрирован в системе");
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [AjaxValidateAntiForgeryToken]
        [System.Web.Http.HttpPost]
        public ResultModel SetPassword(SetPasswordData data)
        {
            if (String.IsNullOrWhiteSpace(data.Password))
            {
                return ResultModel.Error;
            }
            try
            {
                _passwordTicketService.UseTicket(data.Token, data.Password);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        public class SetPasswordData
        {
            public Guid Token { get; set; }
            public string Password { get; set; }
        }

        [System.Web.Http.HttpPost]
        public ResultModel DeleteUser(string name)
        {
            if (User.Identity.Name.ToLower().Equals(name.ToLower()))
            {
                return new ResultModel(false, "Нельзя удалить самого себя");
            }
            try
            {
                _securityService.RemoveUser(name);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [AuthorizeResources(RoCmsResources.Users)]
        [System.Web.Http.HttpPost]
        public ResultModel UpdateResources(UpdateResourcesData data)
        {
            //var user = _securityService.GetUser(data.UserId);
            //if (User.Identity.Name.ToLower().Equals(user.Username.ToLower()))
            //{
            //    return new ResultModel(false, "Нельзя выставлять права себе");
            //}
            try
            {
                _securityService.SetResources(data.UserId, data.ResourceIds);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }

        [AuthorizeResources(RoCmsResources.Users)]
        [System.Web.Http.HttpPost]
        public ResultModel UpdateProfile(User data)
        {

            try
            {
                _securityService.UpdateUser(data);
                return ResultModel.Success;
            }
            catch (Exception e)
            {
                return new ResultModel(e);
            }
        }


        //[System.Web.Mvc.HttpPost]
        //[AuthorizeResources(RoCmsResources.Users)]
        //public ResultModel TurnRegOn()
        //{
        //    try
        //    {
        //        _settingsService.TurnRegOn();
        //        return ResultModel.Success;
        //    }
        //    catch (Exception e)
        //    {
        //        return new ResultModel(false, e.Message);
        //    }
        //}

        //[System.Web.Mvc.HttpPost]
        //[AuthorizeResources(RoCmsResources.Users)]
        //public ResultModel TurnRegOff()
        //{
        //    try
        //    {
        //        _settingsService.TurnRegOff();
        //        return ResultModel.Success;
        //    }
        //    catch (Exception e)
        //    {
        //        return new ResultModel(false, e.Message);
        //    }
        //}

        public class UpdateResourcesData
        {
            public int UserId { get; set; }
            public ICollection<int> ResourceIds { get; set; }
        }



    }
}
