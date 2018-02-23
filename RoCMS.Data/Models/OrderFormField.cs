using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Data.Models
{
    public class OrderFormField
    {
        public int OrderFormFieldId { get; set; }
        public string LabelText { get; set; }
        public string ValueType { get; set; }
        public bool Required { get; set; }
        public int OrderFormId { get; set; }
        public int SortOrder { get; set; }

        public string AcceptableValues { get; set; }
    }
}
