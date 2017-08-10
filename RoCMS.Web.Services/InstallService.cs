using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RoCMS.Data.Gateways;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Web.Services
{
    public class InstallService: IInstallService
    {
        public void Install(string dbScriptPath, string connectionString, string adminLogin, string adminPassword, string rootUrl)
        {
            var script = File.ReadAllText(dbScriptPath, Encoding.UTF8);
            var databaseName = Regex.Match(connectionString, @"initial catalog=(.+?);").Groups[1].Value;
            script = script.Replace("$(DatabaseName)", databaseName)
                .Replace("'admin'",$"'{adminLogin}'")
                .Replace("21232f297a57a5a743894a0e4a801fc3", SecurityService.CalculateHash(adminPassword))
                .Replace("http://localhost:4014", rootUrl);
            script = Regex.Replace(script, "--.*$", "");
            script = Regex.Replace(script, "/\\*.*\\*/ ", "", RegexOptions.Multiline);
            var queryGateway = new QueryGateway(connectionString);
            queryGateway.ExecBatch(script);
        }
        
        public bool CheckDatabase(string connectionString)
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
