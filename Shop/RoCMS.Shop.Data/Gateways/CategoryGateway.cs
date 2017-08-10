using System.Collections.Generic;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class CategoryGateway : ShopBaseGateway
    {
        public Category SelectOne(int id)
        {
            return Exec<Category>(GetProcedureString(), id);
        }
        public Category SelectOneByRelativeUrl(string relativeUrl)
        {
            return Exec<Category>(GetProcedureString(), relativeUrl);
        }

        public ICollection<Category> Select(int? parentCategoryId)
        {
            return ExecSelect<Category>(GetProcedureString(), parentCategoryId);
        }

        public void Update(Category rec)
        {
            Exec(GetProcedureString(), rec);
        }

        public int Insert(Category rec)
        {
            return Exec<int>(GetProcedureString(), rec);
        }

        public void Delete(int id)
        {
            Exec(GetProcedureString(), id);
        }

        public bool Exists(int id)
        {
            return Exec<bool>(GetProcedureString(), id);
        }

        public bool ExistsByRelativeUrl(string relativeUrl)
        {
            return Exec<bool>(GetProcedureString(), relativeUrl);
        }
    }
}
