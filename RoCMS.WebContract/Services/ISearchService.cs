using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Web.Contract.Models;
using RoCMS.Web.Contract.Models.Search;

namespace RoCMS.Web.Contract.Services
{
    public interface ISearchService
    {
        /// <summary>
        /// Регистрация правил индексации типа
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rules"></param>
        void RegisterRules(Type type, ICollection<IndexingRule> rules);
        void UpdateIndex(ISearchable item);
        void RemoveFromIndex(Type type, object id);
        IEnumerable<SearchResultItem> Search(SearchParams searchParams, out int totalCount, int startIndex = 1, int count = 10);
    }
}
