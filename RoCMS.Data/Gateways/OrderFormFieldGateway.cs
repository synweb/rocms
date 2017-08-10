using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class OrderFormFieldGateway:BasicGateway<OrderFormField>
    {
        public override ICollection<OrderFormField> Select()
        {
            throw new NotSupportedException();
        }

        public ICollection<OrderFormField> SelectByForm(int orderFormId)
        {
            return ExecSelect<OrderFormField>(GetProcedureString(), orderFormId);
        } 
    }
}
