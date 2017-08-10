using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models.Search
{
    public class TypeRules
    {
        public TypeRules(ICollection<IndexingRule> indexingRules)
        {
            IndexingRules = indexingRules;
        }

        public ICollection<IndexingRule> IndexingRules { get; }
        
    }
}
