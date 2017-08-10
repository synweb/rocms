using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    class DevelopmentService: IDevelopmentService
    {

        public bool CheckDatabaseCredentials(string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // throws if invalid
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
