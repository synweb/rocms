//#define SEARCH_WITH_CONTAINSTABLE

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base;
using RoCMS.Base.Data;
using RoCMS.Base.Helpers;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class SearchItemGateway:BaseGateway
    {

        public ICollection<SearchResultItem> Find(string searchPattern, ICollection<Type> searchEntities)
        {
            string query;
            if (string.IsNullOrEmpty(searchPattern))
            {
                query = null;
            }
            else
            {
#if (SEARCH_WITH_CONTAINSTABLE)
                var keywords = SearchHelper.ExtractKeywords(searchPattern);
                string format = "FORMSOF(INFLECTIONAL, {0})";
                var strings = keywords.Select(x => string.Format(format, x));
                query = string.Join(" AND ", strings);
#else
                query = searchPattern;
#endif
            }

            var res = ExecSelect<SearchResultItem>(GetProcedureString(), new { FulltextSearchQuery=query, Entities=searchEntities.Select(x => x.FullName) });
            return res;
        }

        public void Upsert(SearchItem item)
        {
            Exec(GetProcedureString(), item, true);
        }

        public void Delete(string entityName, string entityId)
        {
            Exec(GetProcedureString(), new {entityName, entityId}, true);
        }

        public ICollection<SearchResultItem> FindStrict(string searchPattern, ICollection<Type> searchEntities)
        {
            string query;
            if (string.IsNullOrEmpty(searchPattern))
            {
                query = null;
            }
            else
            {
                query = searchPattern;
            }
            var res = ExecSelect<SearchResultItem>(GetProcedureString(), new { SearchQuery = query, Entities = searchEntities.Select(x => x.FullName) });
            return res;
        }
    }
}
