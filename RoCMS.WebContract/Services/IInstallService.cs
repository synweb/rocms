using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Services
{
    public interface IInstallService
    {
        void Install(string dbScriptPath, string connectionString, string adminLogin, string adminPassword, string rootUrl);
        bool CheckDatabase(string connectionString);
    }
}
