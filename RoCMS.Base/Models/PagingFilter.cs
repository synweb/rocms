using System;

namespace RoCMS.Base.Models
{
    [Obsolete]
    public class PagingFilter
    {
        /// <summary>
        /// Все записи на одной странице
        /// </summary>

        public PagingFilter()
        {
            Page = 1;
            PageSize = int.MaxValue;
        }

        public PagingFilter(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

