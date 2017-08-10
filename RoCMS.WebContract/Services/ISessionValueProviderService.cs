using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Services
{
    public interface ISessionValueProviderService
    {
        TValue Get<TValue>(string key);
        void Set<TValue>(string key, TValue value);
    }

}
