using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using RoCMS.Base.Extentions;
using RoCMS.Base.Helpers;

namespace RoCMS.Base.Data
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
            DatabaseFactory.ClearDatabaseProviderFactory();
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
        }

        #region Properties

        protected virtual string ConnectionName => "BasicConnection";
        protected virtual string TableName => GetType().Name.Replace("Gateway", "");
        protected virtual string DefaultScheme => "dbo";

        #endregion

        #region Raw Methods

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

        protected static bool CheckColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == columnName)
                {
                    return true;
                }
            }

            return false;
        }

        protected string GetProcedureString([System.Runtime.CompilerServices.CallerMemberName] string procedureName = null, string scheme = null)
        {
            return $"[{scheme??DefaultScheme}].[{TableName}_{procedureName}]";
        }

        #region Generic Gateway Methods

        #region Readers

        protected T ReadRecord<T>(IDataReader reader)
        {
            Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            if (IsSimple(type)) // простой тип
            {
                var rec = reader[0];
                if (rec == null || rec is DBNull)
                {
                    if (default(T) == null)
                    {
                        return default(T);
                    }
                    else
                    {
                        // если результат - valueType
                        throw new SqlNullValueException();
                    }
                }
                return ReadSimple<T>(reader);
            }
            else // класс, свойства которого нужно заполнить
            {
                var res = FillFromReader(typeof(T), reader);
                return (T)res;
            }
        }

        protected void FillRecordFromReader<T>(T record, IDataReader reader)
        {
            var type = typeof(T);
            if (IsSimple(type))
            {
                throw new ArgumentException("Record must be a custom data class. Use ReadRecord");
            }
            foreach (var propertyInfo in type.GetProperties().Where(x => x.GetValue(record) == null || x.GetValue(record).Equals(GetDefault(x.PropertyType))))
            {
                var propName = propertyInfo.Name;
                if (!CheckColumnExists(reader, propName))
                {
                    continue;
                }
                var dbValue = reader[propName];
                if (dbValue is DBNull)
                {
                    propertyInfo.SetValue(record, null);
                    continue;
                }
                //var value = Convert.ChangeType(dbValue, propertyInfo.PropertyType);
                Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                object safeValue;
                if (t.IsEnum)
                {
                    safeValue = Enum.Parse(t, (string)dbValue);
                }
                else
                {
                    safeValue = Convert.ChangeType(dbValue, t);
                }
                propertyInfo.SetValue(record, safeValue);
            }
        }

        #endregion

        #region Params

        protected void FillSingleParam(object param, DbCommand cmd, Database db)
        {
            var inParams = cmd.Parameters.Cast<DbParameter>().Where(x => x.Direction == ParameterDirection.Input || x.Direction == ParameterDirection.InputOutput).ToArray();
            if (!inParams.Any())
            {
                return;
            }
            var sqlParam = inParams.Single();
            string paramName = sqlParam.ParameterName.Replace("@", "");
            if (param is IEnumerable<string>
                || param is IEnumerable<int>
                || param is IEnumerable<KeyValuePair<int, string>>)
            {
                SetCollectionParameterValue(cmd, db, sqlParam, (IEnumerable) param);
            }
            else
            {
                db.SetParameterValue(cmd, paramName, param);
            }
        }

        private void SetCollectionParameterValue(DbCommand cmd, Database db, DbParameter parameter, IEnumerable paramValue)
        {
            string paramName = parameter.ParameterName.Replace("@", "");
            if (paramValue is IEnumerable<string>)
            {
                DataTable stringTable = new DataTable();
                stringTable.Columns.Add("Val", typeof(string));
                foreach (var str in (IEnumerable<string>)paramValue)
                {
                    var row = stringTable.NewRow();
                    row["Val"] = str;
                    stringTable.Rows.Add(row);
                }
                    (cmd.Parameters[parameter.ParameterName] as SqlParameter).TypeName = "[dbo].[String_Table]";
                db.SetParameterValue(cmd, paramName, stringTable);
            }
            else if (paramValue is IEnumerable<int>)
            {
                DataTable intTable = new DataTable();
                intTable.Columns.Add("Val", typeof(int));
                foreach (var num in (IEnumerable<int>)paramValue)
                {
                    var row = intTable.NewRow();
                    row["Val"] = num;
                    intTable.Rows.Add(row);
                }
                (cmd.Parameters[parameter.ParameterName] as SqlParameter).TypeName = "[dbo].[Int_Table]";
                db.SetParameterValue(cmd, paramName, intTable);
            }
            else if (paramValue is IEnumerable<KeyValuePair<int, string>>)
            {
                DataTable intStringTable = new DataTable();
                intStringTable.Columns.Add("Key", typeof(int));
                intStringTable.Columns.Add("Val", typeof(string));
                foreach (var num in (IEnumerable<KeyValuePair<int, string>>)paramValue)
                {
                    var row = intStringTable.NewRow();
                    row["Key"] = num.Key;
                    row["Val"] = num.Value;
                    intStringTable.Rows.Add(row);
                }
                (cmd.Parameters[parameter.ParameterName] as SqlParameter).TypeName = "[dbo].[Int_String_Table]";
                db.SetParameterValue(cmd, paramName, intStringTable);
            }
        }

        protected void FillParams(IDictionary<string, object> dictionary, DbCommand cmd, Database db)
        {
            var inParams =
                cmd.Parameters.Cast<DbParameter>()
                    .Where(x => x.Direction == ParameterDirection.Input || x.Direction == ParameterDirection.InputOutput)
                    .ToArray();
            if (!inParams.Any())
            {
                return;
            }
            if (inParams.Count() > dictionary.Count)
            {
                throw new Exception("param count");
            }
            foreach (DbParameter parameter in inParams)
            {
                string paramName = parameter.ParameterName.Replace("@", "");
                var paramValue = dictionary[paramName];
                if (paramValue is IEnumerable<string>
                    || paramValue is IEnumerable<int>
                    || paramValue is IEnumerable<KeyValuePair<int, string>>)
                {
                    SetCollectionParameterValue(cmd, db, parameter, (IEnumerable) paramValue);
                }
                else if (paramValue is Dictionary<string, string>)
                {
                    string jsonValue = DataContractSerializeHelper.SerializeJson((Dictionary<string, string>)paramValue);
                    db.SetParameterValue(cmd, paramName, jsonValue);
                }
                else
                {
                    db.SetParameterValue(cmd, paramName, paramValue);
                }
            }

            var outParams =
                cmd.Parameters.Cast<DbParameter>()
                    .Where(x => x.Direction == ParameterDirection.Output)
                    .ToArray();
            foreach (var parameter in outParams)
            {
                string paramName = parameter.ParameterName.Replace("@", "");
                db.SetParameterValue(cmd, paramName, null);
            }
        }

        protected void FillParams(object record, DbCommand cmd, Database db)
        {
            if (record == null)
            {
                FillSingleParam(null, cmd, db);
                return;
            }

            var paramType = Nullable.GetUnderlyingType(record.GetType()) ?? record.GetType();
            if (IsSimple(paramType) || record is IEnumerable)
            {
                FillSingleParam(record, cmd, db);
                return;
            }

            var dictionary = GetParamDictionary(record);
            FillParams(dictionary, cmd, db);
        }

        protected void FillOutParamResults(object record, DbCommand cmd, Database db)
        {
            var outParams =
                cmd.Parameters.Cast<DbParameter>()
                    .Where(x => x.Direction == ParameterDirection.Output || x.Direction == ParameterDirection.InputOutput)
                    .ToArray();
            if(!outParams.Any())
                return;

            var paramType = record.GetType();
            var props = paramType.GetProperties();
            foreach (DbParameter parameter in outParams)
            {
                string paramName = parameter.ParameterName.Replace("@", "");
                var paramValue = db.GetParameterValue(cmd, paramName);

                var prop = props.SingleOrDefault(x => x.Name.Equals(paramName, StringComparison.InvariantCultureIgnoreCase));
                if(prop == null)
                    continue;
                Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                object safeValue = Convert.ChangeType(paramValue, t);
                prop.SetValue(record, safeValue);
            }
        }

        #endregion

        #region Common

        protected ICollection<T> ExecSelect<T>(string procedureName, object @param = null)
        {
            var res = new List<T>();
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                FillParams(@param, cmd, db);
                using (var reader = ExecuteReader(db, cmd))
                {
                    while (reader.Read())
                    {
                        res.Add(ReadRecord<T>(reader));
                    }
                }
                FillOutParamResults(@param, cmd, db);
                return res;
            }
        }

        protected void Exec(string procedureName, object @param = null, bool mayAffectNoRows = false)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                FillParams(@param, cmd, db);
                int res = ExecuteNonQuery(db, cmd);
                if (!mayAffectNoRows && res == 0)
                {
                    throw new Exception("Failed to execute " + procedureName);
                }
                FillOutParamResults(@param, cmd, db);
            }
        }

        protected T Exec<T>(string procedureName, object @param = null)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                T result;
                FillParams(@param, cmd, db);
                using (var reader = ExecuteReader(db, cmd))
                {
                    
                    if (reader.Read())
                    {
                        result = ReadRecord<T>(reader);
                    }
                    else
                    {
                        if (default(T) != null)
                        {
                            throw new SqlNullValueException();
                        }
                        else
                        {
                            result = default(T);
                        }
                    }
                }
                FillOutParamResults(@param, cmd, db);
                return result;
            }
        }

        #endregion

        #endregion

        #region Private

        private T FillFromReader<T>(IDataReader reader) where T : new()
        {
            return (T)FillFromReader(typeof(T), reader);
        }

        /// <summary>
        /// Если недоступен generic-метод
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object FillFromReader(Type type, IDataReader reader)
        {
            Type resType = Nullable.GetUnderlyingType(type) ?? type;
            if (IsSimple(resType))
            {
                return ReadSimple(type, reader);
            }
            var res = resType.GetConstructor(new Type[] { }).Invoke(new object[] { });
            foreach (var propertyInfo in type.GetProperties())
            {
                var propName = propertyInfo.Name;
                if (!CheckColumnExists(reader, propName))
                {
                    continue;
                }
                var dbValue = reader[propName];
                if (dbValue is DBNull)
                {
                    propertyInfo.SetValue(res, null);
                    continue;
                }
                bool isJson = propertyInfo.PropertyType == typeof(Dictionary<string, string>);
                if (isJson)
                {
                    var dict = DataContractSerializeHelper.DeserializeJson(Convert.ToString(dbValue));
                    propertyInfo.SetValue(res, dict);
                }
                else if (propertyInfo.PropertyType.IsEnum)
                {
                    var value = Enum.Parse(propertyInfo.PropertyType, Convert.ToString(dbValue));
                    propertyInfo.SetValue(res, value);
                }
                else
                {
                    Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                    if (t.IsEnum)
                    {
                        var value = Enum.Parse(t, Convert.ToString(dbValue));
                        propertyInfo.SetValue(res, value);
                    }
                    else
                    {
                        object safeValue = Convert.ChangeType(dbValue, t);
                        propertyInfo.SetValue(res, safeValue);
                    }
                }
            }
            return res;
        }

        private T ReadSimple<T>(IDataReader reader)
        {
            var rec = reader[0];
            var type = typeof(T);
            Type t = Nullable.GetUnderlyingType(type) ?? type;
            object safeValue = Convert.ChangeType(rec, t);
            return (T)safeValue;
        }


        private object ReadSimple(Type type, IDataReader reader)
        {
            var rec = reader[0];
            Type t = Nullable.GetUnderlyingType(type) ?? type;
            object safeValue = Convert.ChangeType(rec, t);
            return safeValue;
        }

        private static bool IsSimple(Type type)
        {
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || type == typeof(decimal)
                   || type == typeof(Guid)
                   ;
        }

        private static Dictionary<string, object> GetParamDictionary(object param)
        {
            var paramType = @param.GetType();
            var props = paramType.GetProperties();
            var dictionary = props.ToDictionary(x => x.Name.UppercaseFirst(), x => x.GetValue(@param, null));
            return dictionary;
        }

        private static T GetDefault<T>()
        {
            return (T)GetDefault(typeof(T));
        }


        private static object GetDefault(Type type)
        {
            // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
            if (type == null || !type.IsValueType || type == typeof(void))
                return null;

            // If the supplied Type has generic parameters, its default value cannot be determined
            if (type.ContainsGenericParameters)
                throw new ArgumentException(
                    "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                    "> contains generic parameters, so the default value cannot be retrieved");

            // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a 
            //  default instance of the value type
            if (type.IsPrimitive || !type.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + type +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }

            // Fail with exception
            throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                                        "> is not a publicly-visible type, so the default value cannot be retrieved");
        }

        #endregion

        #region Obsolete

        [Obsolete("Use Exec")]
        protected void ExecForRecord(string procedureName, object record)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                FillParams(record, cmd, db);
                int res = ExecuteNonQuery(db, cmd);
                if (res == 0)
                {
                    throw new Exception("Failed to execute " + procedureName);
                }
            }
        }

        [Obsolete("Use Exec")]
        protected T ExecForRecord<T>(string procedureName, object record)
        {
            var type = typeof(T);
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                FillParams(record, cmd, db);
                var res = ExecuteScalar(db, cmd);

                if (res == null || res is DBNull)
                {
                    return default(T);
                }
                Type t = Nullable.GetUnderlyingType(type) ?? type;
                object safeValue = Convert.ChangeType(res, t);
                return (T)safeValue;
            }
        }

        /// <summary>
        /// Выполнить процедуру с несколькими параметрами
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="params"></param>
        [Obsolete("Use Exec")]
        protected T ExecForParams<T>(string procedureName, IDictionary<string, object> @params)
        {
            var type = typeof(T);
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                var inParams = cmd.Parameters.Cast<DbParameter>().Where(x => x.Direction == ParameterDirection.Input || x.Direction == ParameterDirection.InputOutput).ToArray();
                if (inParams.Count() != @params.Count)
                {
                    throw new Exception("Keys doesn't match");
                }
                foreach (var @param in @params)
                {
                    db.SetParameterValue(cmd, @param.Key, @param.Value);
                }
                var res = ExecuteScalar(db, cmd);

                if (res == null || res is DBNull)
                {
                    return default(T);
                }
                Type t = Nullable.GetUnderlyingType(type) ?? type;
                object safeValue = Convert.ChangeType(res, t);
                return (T)safeValue;
            }
        }

        /// <summary>
        /// Выполнить процедуру с единственным параметром
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        [Obsolete("Use Exec")]
        protected void ExecForParam(string procedureName, object @param)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                var inParams = cmd.Parameters.Cast<DbParameter>().Where(x => x.Direction == ParameterDirection.Input || x.Direction == ParameterDirection.InputOutput).ToArray();
                if (inParams.Count() > 1)
                {
                    throw new Exception("There are more than one key");
                }
                db.SetParameterValue(cmd, inParams.Single().ParameterName, @param);
                int res = ExecuteNonQuery(db, cmd);
                if (res == 0)
                {
                    throw new Exception("Failed to execute " + procedureName);
                }
            }
        }

        /// <summary>
        /// Выполнить процедуру с единственным параметром
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        [Obsolete("Use Exec")]
        protected T ExecForParam<T>(string procedureName, object @param)
        {
            var type = typeof(T);
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                var inParams = cmd.Parameters.Cast<DbParameter>().Where(x => x.Direction == ParameterDirection.Input || x.Direction == ParameterDirection.InputOutput).ToArray();
                if (inParams.Count() > 1)
                {
                    throw new Exception("There are more than one key");
                }
                db.SetParameterValue(cmd, inParams.Single().ParameterName, @param);
                var res = ExecuteScalar(db, cmd);

                if (res == null || res is DBNull)
                {
                    return default(T);
                }
                Type t = Nullable.GetUnderlyingType(type) ?? type;
                object safeValue = Convert.ChangeType(res, t);
                return (T)safeValue;
            }
        }

        /// <summary>
        /// Выполнить процедуру с несколькими параметрами
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="params"></param>
        [Obsolete("Use Exec")]
        protected void ExecForParams(string procedureName, IDictionary<string, object> @params)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                var inParams = cmd.Parameters.Cast<DbParameter>().Where(x => x.Direction == ParameterDirection.Input || x.Direction == ParameterDirection.InputOutput).ToArray();
                if (inParams.Count() != @params.Count)
                {
                    throw new Exception("Keys doesn't match");
                }
                foreach (var @param in @params)
                {
                    db.SetParameterValue(cmd, @param.Key, @param.Value);
                }
                int res = ExecuteNonQuery(db, cmd);
                if (res == 0)
                {
                    throw new Exception("Failed to execute " + procedureName);
                }
            }
        }

        /// <summary>
        /// Выполнить процедуру без параметров
        /// </summary>
        /// <param name="procedureName"></param>
        [Obsolete("Use Exec")]
        protected T ExecScalar<T>(string procedureName)
        {
            var type = typeof(T);
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                var res = ExecuteScalar(db, cmd);

                if (res == null || res is DBNull)
                {
                    return default(T);
                }
                Type t = Nullable.GetUnderlyingType(type) ?? type;
                object safeValue = Convert.ChangeType(res, t);
                return (T)safeValue;
            }
        }

        /// <summary>
        /// Выполнение SelectOne для процедуры без параметров
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        [Obsolete("Use Exec")]
        protected T ExecSelectOne<T>(string procedureName) where T : new()
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                using (var reader = ExecuteReader(db, cmd))
                {
                    if (reader.Read())
                    {
                        return ReadRecord<T>(reader);
                    }
                    throw new RowNotInTableException();
                }
            }
        }


        /// <summary>
        /// Выполнение SelectOne для процедуры с единственным параметром
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [Obsolete("Use Exec")]
        protected T ExecSelectOne<T>(string procedureName, object @param) where T : new()
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                var inParams = cmd.Parameters.Cast<DbParameter>().Where(x => x.Direction == ParameterDirection.Input || x.Direction == ParameterDirection.InputOutput).ToArray();
                if (inParams.Count() > 1)
                {
                    throw new Exception("There are more than one key");
                }
                db.SetParameterValue(cmd, inParams.Single().ParameterName, @param);
                using (var reader = ExecuteReader(db, cmd))
                {
                    if (reader.Read())
                    {
                        return ReadRecord<T>(reader);
                    }
                    throw new RowNotInTableException();
                }
            }
        }

        /// <summary>
        /// Выполнение SelectOne для процедуры с несколькими параметрами
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        [Obsolete("Use Exec")]
        protected T ExecSelectOne<T>(string procedureName, IDictionary<string, object> @params) where T : new()
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                foreach (var @param in @params)
                {
                    db.SetParameterValue(cmd, @param.Key, @param.Value);
                }
                using (var reader = ExecuteReader(db, cmd))
                {
                    if (reader.Read())
                    {
                        return ReadRecord<T>(reader);
                    }
                    return default(T);
                    //throw new RowNotInTableException();
                }
            }
        }

        [Obsolete("Use Exec without Dictionary")]
        protected T Exec<T>(string procedureName, IDictionary<string, object> @params)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                FillParams(@params, cmd, db);
                using (var reader = ExecuteReader(db, cmd))
                {
                    if (reader.Read())
                    {
                        return ReadRecord<T>(reader);
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
        }

        [Obsolete("Use ExecSelect without Dictionary")]
        protected ICollection<T> ExecSelect<T>(string procedureName, IDictionary<string, object> @params)
        {
            var res = new List<T>();
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                FillParams(@params, cmd, db);
                using (var reader = ExecuteReader(db, cmd))
                {
                    {
                        while (reader.Read())
                        {
                            res.Add(ReadRecord<T>(reader));
                        }
                    }
                }
                return res;
            }
        }

        [Obsolete("Use Exec without Dictionary")]
        protected void Exec(string procedureName, IDictionary<string, object> @params)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand(procedureName, db))
            {
                FillParams(@params, cmd, db);
                int res = ExecuteNonQuery(db, cmd);
                if (res == 0)
                {
                    throw new Exception("Failed to execute " + procedureName);
                }
            }
        }

        #endregion
    }
}
