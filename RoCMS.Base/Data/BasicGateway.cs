using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Base.Data
{
    /// <summary>
    /// Gateway, содержащий методы для основных CRUD-хранимок
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicGateway<T> : BaseGateway where T : class, new()
    {
        public virtual ICollection<T> Select()
        {
            return ExecSelect<T>($"[{DefaultScheme}].[{TableName}_Select]");
        }

        public virtual T SelectOne(int id)
        {
            return Exec<T>($"[{DefaultScheme}].[{TableName}_SelectOne]", id);
        }

        public virtual int Insert(T record)
        {
            return Exec<int>($"[{DefaultScheme}].[{TableName}_Insert]", record);
        }

        public virtual void Delete(int id)
        {
            Exec($"[{DefaultScheme}].[{TableName}_Delete]", id);
        }

        public virtual void Update(T record)
        {
            Exec($"[{DefaultScheme}].[{TableName}_Update]", record);
        }
    }
}
