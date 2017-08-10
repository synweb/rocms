using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models.Search
{
    public interface ISearchable
    {
        IEnumerable<string> SearchIndexKeys { get; } 
    }
}
