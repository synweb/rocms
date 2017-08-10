using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoCMS.Base.Models;

namespace RoCMS.News.Contract.Models
{
    public class NewsFilter
    {
        public NewsFilter()
        {
            RecordTypes = new List<RecordType>();
            OnlyPosted = true;
            SortBy = NewsItemSortBy.PostingDate;
            SortOrder = SortOrder.Desc;
        }

        public bool OnlyPosted { get; set; }
        public bool? OnlyFutureEventDate { get; set; }

        /// <summary>
        /// Логический (операции И и ИЛИ) фильтр по категориям в дизъюнктивной форме. Пример: "1,2,3;5,6;9" => (1|2|3) & (5|6) & 9
        /// </summary>
        public string CategoryQuery { get; set; }

        /// <summary>
        /// Полнотекстовый поисковый запрос
        /// </summary>
        public string TextQuery { get; set; }


        public ICollection<RecordType> RecordTypes { get; set; }

        public string TagName { get; set; }

        public int? BlogId { get; set; }

        public NewsItemSortBy SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}
