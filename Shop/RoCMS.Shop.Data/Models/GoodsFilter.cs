using System.Collections.Generic;

namespace RoCMS.Shop.Data.Models
{
    public class GoodsFilter
    {
        public GoodsFilter()
        {
            //CategoryIds = new List<int>();
            CategoryIds = new List<List<int>>();
            ManufacturerIds = new List<int>();
            SupplierIds = new List<int>();
            Countries = new List<int>();
            PackIds = new List<int>();
            ActionIds = new List<int>();
            Articles = new List<string>();
            SpecIds = new Dictionary<int, string>();
        }
        //public IEnumerable<int> CategoryIds { get; set; }
        public IEnumerable<IEnumerable<int>> CategoryIds { get; set; }
        /// <summary>
        /// Если true, то вынимаются товары из всех дочерних категорий, кроме скрытых.
        /// Товары скрытых категорий не возвращаются
        /// </summary>
        public bool WithSubcategories { get; set; }

        /// <summary>
        /// Точные совпадения по значениям характеристик
        /// </summary>
        public IDictionary<int, string> SpecIds { get; set; }

        public IEnumerable<int> ManufacturerIds { get; set; }
        public IEnumerable<int> SupplierIds { get; set; }

        public IEnumerable<string> Articles { get; set; }

        public IEnumerable<int> Countries { get; set; }

        public IEnumerable<int> PackIds { get; set; }

        public IEnumerable<int> ActionIds { get; set; }

        /// <summary>
        /// Текстовый запрос поиска
        /// </summary>
        public string SearchPattern { get; set; }

        /// <summary>
        /// Сортировка производится по: ...
        /// </summary>
        public SortCriterion SortBy { get; set; }
    }
}
