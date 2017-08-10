using System.Collections.Generic;
using RoCMS.Base.Models;
using RoCMS.Web.Contract.Models;

namespace RoCMS.Shop.Contract.Models
{
    public class CompatibleSet
    {
        public int CompatibleSetId { get; set; }
        public string Name { get; set; }
        public System.Guid Guid { get; set; }

        public List<IdNamePair<int>> CompatibleGoods { get; set; }
    }
}
