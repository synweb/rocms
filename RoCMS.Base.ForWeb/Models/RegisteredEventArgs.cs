using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RoCMS.Base.ForWeb.Models
{
    public class RegisteredEventArgs: EventArgs
    {
        public RegisteredEventArgs(HttpContext httpContext, int userId)
        {
            UserId = userId;
            HttpContext = httpContext;
        }

        public HttpContext HttpContext { get; private set; }
        public int UserId { get; private set; }
    }
}
