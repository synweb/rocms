using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models.Search
{
    public class SearchParams
    {
        /// <summary>
        /// Параметры поиска
        /// </summary>
        /// <param name="searchPattern">Текстовый запрос</param>
        /// <param name="searchEntities">Типы, которые требуется искать</param>
        public SearchParams(string searchPattern, ICollection<Type> searchEntities=null)
        {
            SearchPattern = searchPattern;
            SearchEntities = searchEntities ?? new List<Type>();
        }

        /// <summary>
        /// Текстовый запрос
        /// </summary>
        public string SearchPattern { get; set; }

        /// <summary>
        /// Типы, которые требуется искать
        /// </summary>
        public ICollection<Type> SearchEntities { get; set; } 
    }
}
