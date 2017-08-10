using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace RoCMS.Comments.Data.Gateways
{
    public abstract class BaseGateway
    {
        #region Fields

        volatile Database _db;

        static readonly ParameterCache parameterCache = new ParameterCache();
        static readonly DateTime MinSqlDateValue = SqlDateTime.MinValue.Value;
        static readonly DateTime MaxSqlDateValue = SqlDateTime.MaxValue.Value;

        #endregion

        static BaseGateway()
        {
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory()); 
        }

        #region Properties

        string ConnectionName
        {
            get { return "CommentsConnection"; }
        }

        #endregion

        #region Methods

        protected Database GetDb()
        {
            if (_db == null)
            {
                lock (this)
                {
                    if (_db == null)
                    {
                        _db = DatabaseFactory.CreateDatabase(ConnectionName);
                    }
                }
            }
            return _db;
        }

        protected DbCommand GetCommand(String cmd, Database db)
        {
            DbCommand res = db.GetStoredProcCommand(cmd);
            parameterCache.SetParameters(res, db);

            return res;
        }

        protected int ExecuteNonQuery(Database db, DbCommand cmd)
        {
            return db.ExecuteNonQuery(cmd);
        }

        protected Object ExecuteScalar(Database db, DbCommand cmd)
        {
            return db.ExecuteScalar(cmd);
        }

        protected IDataReader ExecuteReader(Database db, DbCommand cmd)
        {

            return db.ExecuteReader(cmd);
        }

        #endregion

        protected static DateTime? NormalizeDateValue(DateTime? dateTime)
        {
            return dateTime != null ? NormalizeDateValue(dateTime.GetValueOrDefault()) : default(DateTime?);
        }

        protected static DateTime NormalizeDateValue(DateTime dateTime)
        {
            if (dateTime < MinSqlDateValue)
                return MinSqlDateValue;
            if (dateTime > MaxSqlDateValue)
                return MaxSqlDateValue;
            return dateTime;
        }
    }
}
