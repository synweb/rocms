using System;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RoCMS.Base.ForWeb.Models;
using RoCMS.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Security;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Helpers
{
    public class AuthenticationHelper
    {
        private static ISecurityService _securityService;
        private static AuthenticationHelper _authenticationHelper;
        private static readonly object _lockObj = new object();

        public event Action<AuthenticatedEventArgs> Authenticated = (e) => { };
        public event Action<RegisteredEventArgs> Registered = (e) => { };

        private AuthenticationHelper(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public static AuthenticationHelper GetInstance()
        {
            if (_securityService == null)
            {
                lock (_lockObj)
                {
                    if (_securityService == null)
                    {
                        _authenticationHelper = new AuthenticationHelper(DependencyResolver.Current.GetService<ISecurityService>());
                    }
                }
            }
            return _authenticationHelper;
        }

        /// <summary>
        /// Аутентификация. Возвращает id пользователя в случае успеха.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>UserId</returns>
        public int Login(HttpContext httpContext, string username, string password)
        {
            var pwd = password.Trim();
            var user = username.Trim();
            bool success = _securityService.Authenticate(user, pwd);
            if (success)
            {
                User nUser = _securityService.GetUser(user);
                // "key:val,key:val,..."
                string userData = string.Format("id:{0}",nUser.UserId);

                var ticket = new FormsAuthenticationTicket(2, user, DateTime.Now, DateTime.Now.AddYears(5),
                    true, userData);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                authCookie.Expires = DateTime.Now.AddYears(5);
                httpContext.Response.Cookies.Add(authCookie);
                Authenticated(new AuthenticatedEventArgs(httpContext, nUser.UserId));
                return nUser.UserId;
            }
            throw new AuthenticationException();
        }

        public int GetUserId(HttpContext httpContext)
        {
            try
            {
                RoPrincipal user = httpContext.User as RoPrincipal;
                if (user != null)
                {
                    return user.UserId;
                }
                var cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                var data = ticket.UserData.Split(';');
                foreach (var kvp in data)
                {
                    var kv = kvp.Split(':');
                    if (kv[0] == "id")
                    {
                        return int.Parse(kv[1]);
                    }
                }
            }
            catch
            {
                // ignored
            }
            FormsAuthentication.SignOut();
            throw new AuthenticationException("User not authenticated");
        }

        /// <summary>
        /// Возвращает id пользователя в случае успеха.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns>UserId</returns>
        public int RegisterUser(HttpContext context, string username, string password, string email=null)
        {
            var usr = username.Trim();
            var pwd = password.Trim();
            var em = email?.Trim();
            _securityService.RegisterUser(usr, pwd, em);
            int id = _securityService.GetUser(usr).UserId;
            Registered(new RegisteredEventArgs(context, id));
            return id;
        }
    }
}