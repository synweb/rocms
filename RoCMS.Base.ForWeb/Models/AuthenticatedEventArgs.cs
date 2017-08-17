using System;
using System.Web;

namespace RoCMS.Models
{
    public class AuthenticatedEventArgs:EventArgs
    {
        public AuthenticatedEventArgs(HttpContext httpContext, int userId)
        {
            UserId = userId;
            HttpContext = httpContext;
        }

        public HttpContext HttpContext { get; private set; }
        public int UserId { get; private set; }
    }
}