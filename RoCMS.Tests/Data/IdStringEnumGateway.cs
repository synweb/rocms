using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;

namespace RoCMS.Tests.Data
{
    public class IdStringEnumGateway: BasicGateway<IdStringEnum>
    {
        public IdStringEnum SelectOneWithExec(int id)
        {
            return Exec<IdStringEnum>(GetProcedureString(nameof(SelectOne)), id);
        }

        public DbEnum? SelectNothingAsNullableEnum()
        {
            return Exec<DbEnum?>("SelectNothing");
        }

        public DbEnum SelectNothingAsEnum()
        {
            return Exec<DbEnum>("SelectNothing");
        }
    }
}
