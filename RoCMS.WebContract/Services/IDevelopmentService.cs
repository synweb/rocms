using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Web.Contract.Services
{
    public interface IDevelopmentService
    {
        bool CheckDatabaseCredentials(string connectionString);
    }
}
