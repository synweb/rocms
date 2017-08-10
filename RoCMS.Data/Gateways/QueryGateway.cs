using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RoCMS.Base.Data;

namespace RoCMS.Data.Gateways
{
    public class QueryGateway : BaseGateway
    {
        public QueryGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly string _connectionString;

        /// <summary>
        /// Выполнение небольшого скрипта
        /// </summary>
        /// <param name="script"></param>
        public void Exec(string script)
        {
            var db = new SqlDatabase(_connectionString);
            using (var connection = db.CreateConnection())
            using (var cmd = db.GetSqlStringCommand(script))
            {
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// Выполнение большого скрипта, содержащего инструкции GO
        /// </summary>
        /// <param name="script"></param>
        public void ExecBatch(string script)
        {
            var db = new SqlDatabase(_connectionString);
            using (var connection = db.CreateConnection())
            using (var cmd = connection.CreateCommand())
            {
                cmd.Connection = connection;
                connection.Open();
                foreach (string part in script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    {
                        cmd.CommandText = part;
                        cmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }
    }
}
