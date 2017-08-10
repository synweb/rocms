using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Shop.Contract.Models
{
    public class Currency
    {
        public string CurrencyId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public decimal Rate { get; set; }
        public int SortOrder { get; set; }
        public bool IsMain { get; set; }
    }
}
