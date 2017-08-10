using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class HttpSessionValueProviderService : ISessionValueProviderService
    {
        #region ISessionValueProviderService Members

        public TValue Get<TValue>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key");
            }

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                return (TValue)HttpContext.Current.Session[key];
            }
            else
            {
                return default(TValue);
            }
        }

        public void Set<TValue>(string key, TValue value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key");
            }

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        #endregion
    }
}
