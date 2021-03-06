﻿using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RoCMS.Shop.Contract.Models
{
    public class GoodsFilter
    {
        public GoodsFilter()
        {
            CategoryIds = new List<List<int>>();
            ManufacturerIds = new List<int>();
            SupplierIds = new List<int>();
            Countries = new List<int>();
            Packs = new List<int>();
            ActionIds = new List<int>();
            Articles = new List<string>();
            SpecIdValues = new Dictionary<int, string>();
        }
        
        /// <summary>
        /// Множества категорий: [[1,2],[3,4]] = (1|2)&(3,4)
        /// </summary>
        public IEnumerable<IEnumerable<int>> CategoryIds { get; set; }

        /// <summary>
        /// Если true, то вынимаются товары из всех дочерних категорий, кроме скрытых.
        /// Товары скрытых категорий не возвращаются
        /// </summary>
        public bool? ClientMode{ get; set; }

        /// <summary>
        /// Точные совпадения по значениям характеристик
        /// </summary>
        public IDictionary<int, string> SpecIdValues { get; set;}

        public IEnumerable<int> ManufacturerIds { get; set; }
        public IEnumerable<int> SupplierIds { get; set; }

        public IEnumerable<string> Articles { get; set; }

        public IEnumerable<int> Countries { get; set; }

        public IEnumerable<int> Packs { get; set; }
        
        public IEnumerable<int> ActionIds { get; set; }

        /// <summary>
        /// Текстовый запрос поиска
        /// </summary>
        public string SearchPattern { get; set; }

        /// <summary>
        /// Сортировка производится по: ...
        /// </summary>
        public SortCriterion SortBy { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
