using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;

namespace RoCMS.Tests.Data
{
    public class IdValueGateway: BasicGateway<IdValue>
    {
        public ICollection<IdValue> SelectWithIdLessThan(int lessThan)
        {
            return ExecSelect<IdValue>(GetProcedureString(nameof(SelectWithIdLessThan)), lessThan);
        }
        public ICollection<IdValue> SelectByIdAndValue(int id, string value)
        {
            return ExecSelect<IdValue>(GetProcedureString(nameof(SelectByIdAndValue)), new {id, value});
        }

        public int SelectNine()
        {
            return Exec<int>((nameof(SelectNine)));
        }

        public int? SelectNullOrFive(bool @null)
        {
            return Exec<int?>((nameof(SelectNullOrFive)), @null);
        }

        public int? SelectNullableIntParam(int? param)
        {
            return Exec<int?>((nameof(SelectNullableIntParam)), param);
        }

        public ICollection<int?> SelectIdsWithNull()
        {
            return ExecSelect<int?>(GetProcedureString(nameof(SelectIdsWithNull)));
        }

        public int? SelectNullOrFiveWithSelectOne(bool @null)
        {
            return Exec<int?>((nameof(SelectNullOrFive)), @null);
        }

        public int? SelectNothingAsNullbale()
        {
            return Exec<int?>("SelectNothing");
        }
        public int SelectNothingAsInt()
        {
            return Exec<int>("SelectNothing");
        }
        public IdValue SelectNothingAsObject()
        {
            return Exec<IdValue>("SelectNothing");
        }
    }
}
