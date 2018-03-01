using System.Collections.Generic;
using System.Linq;
using RoCMS.Base.Helpers;
using RoCMS.Base.Models;
using RoCMS.Shop.Data.Models;

namespace RoCMS.Shop.Data.Gateways
{
    public class GoodsItemGateway: ShopBasicGateway<GoodsItem>
    {
        public ICollection<int> FilterGoods(GoodsFilter filter, int startIndex, int count, out int totalCount)
        {
            string sortOrder = filter.SortBy.ToString().EndsWith("Desc") ? "DESC" : "ASC";
            string sortBy = filter.SortBy.ToString().Replace("Asc", "").Replace("Desc", "");
            int total = 0;
            List<int> ids = null;
            totalCount = -1; // переопределится при вызове хранимки
            if (filter.CategoryIds == null || !filter.CategoryIds.Any())
            {
                // если по категориям не фильтруем
                var param = new GoodsFilterParam()
                {
                    ActionIds = filter.ActionIds,
                    Articles = filter.Articles,
                    Countries = filter.Countries,
                    CategoryIds = new List<int>(),
                    ManufacturerIds = filter.ManufacturerIds,
                    SearchQuery = filter.SearchPattern,
                    FulltextSearchQuery = GetFulltextSearchQuery(filter.SearchPattern),
                    SortBy = sortBy,
                    SpecIds = filter.SpecIds,
                    PackIds = filter.PackIds,
                    SupplierIds = filter.SupplierIds,
                    SortOrder = sortOrder,
                    StartIndex = startIndex,
                    TotalCount = total,
                    Count = count
                };
                var procedureResult = ExecSelect<int>("[Shop].[Goods_Filter]", param);
                totalCount = param.TotalCount;
                return procedureResult;
            }

            // если по категориям фильтруем
            foreach (var catGroup in filter.CategoryIds)
            {
                var param = new GoodsFilterParam()
                {
                    ActionIds = filter.ActionIds,
                    Articles = filter.Articles,
                    CategoryIds = catGroup,
                    WithSubcategories = filter.WithSubcategories,
                    Countries = filter.Countries,
                    ManufacturerIds = filter.ManufacturerIds,
                    SearchQuery = filter.SearchPattern,
                    FulltextSearchQuery = GetFulltextSearchQuery(filter.SearchPattern),
                    SortBy = sortBy,
                    SpecIds = filter.SpecIds,
                    PackIds = filter.PackIds,
                    SupplierIds = filter.SupplierIds,
                    SortOrder = sortOrder,
                    StartIndex = startIndex,
                    TotalCount = total,
                    Count = count
                };
                var procedureResult = ExecSelect<int>("[Shop].[Goods_Filter]", param);
                if (totalCount == -1)
                {
                    totalCount = param.TotalCount;
                }
                if (ids == null)
                {
                    ids = new List<int>(procedureResult);
                }
                else
                {
                    ids = ids.Intersect(procedureResult).ToList();
                }
                if (!ids.Any())
                    break;
            }
            return ids??new List<int>();
        }

        private class GoodsFilterParam
        {
            public IEnumerable<int> ActionIds { get; set; }
            public IEnumerable<string> Articles { get; set; }
            public IEnumerable<int> CategoryIds { get; set; }
            public bool WithSubcategories { get; set; }
            public IEnumerable<int> Countries { get; set; }
            public IEnumerable<int> ManufacturerIds { get; set; }
            public string SearchQuery { get; set; }
            public string FulltextSearchQuery { get; set; }
            public string SortBy { get; set; }
            public IDictionary<int, string> SpecIds { get; set; }
            public IEnumerable<int> PackIds { get; set; }
            public IEnumerable<int> SupplierIds { get; set; }
            public string SortOrder { get; set; }
            public int StartIndex { get; set; }
            public int TotalCount { get; set; }
            public int Count { get; set; }
        }

        public ICollection<IdNamePair<int>> GetManufacturers(ICollection<int> heartIds)
        {
            return ExecSelect<IdNamePair<int>>("[Shop].[Goods_GetManufacturers]", heartIds);
        }

        public ICollection<IdNamePair<int>> GetCountries(ICollection<int> heartIds)
        {
            return ExecSelect<IdNamePair<int>>("[Shop].[Goods_GetCountries]", heartIds);
        }

        public ICollection<IdNamePair<int>> GetPacks(ICollection<int> heartIds)
        {
            return ExecSelect<IdNamePair<int>>("[Shop].[Goods_GetPacks]", heartIds);
        }

        public ICollection<KeyValuePair<int, string>> GetSpecs(ICollection<int> heartIds)
        {
            var res = ExecSelect<GetSpecsResult>("[Shop].[Goods_GetSpecs]", heartIds);
            return res.Select(x => new KeyValuePair<int, string>(x.Key, x.Value)).ToList();
        }

        private class GetSpecsResult
        {
            public int Key { get; set; }
            public string Value { get; set; }
        }

        private string GetFulltextSearchQuery(string searchPattern)
        {
            if (string.IsNullOrEmpty(searchPattern))
            {
                return null;
            }
            var keywords = SearchHelper.ExtractKeywords(searchPattern);
            string format = "FORMSOF(INFLECTIONAL, {0})";
            var strings = keywords.Select(x => string.Format(format, x));
            var res = string.Join(" AND ", strings);
            return res;
        }

        public bool Exists(int id)
        {
            return Exec<bool>(GetProcedureString(), id);
        }

        //public bool ExistsByRelativeUrl(string relativeUrl)
        //{
        //    return Exec<bool>(GetProcedureString(), relativeUrl);
        //}

        //public GoodsItem SelectOneByRelativeUrl(string relativeUrl)
        //{
        //    return Exec<GoodsItem>(GetProcedureString(), relativeUrl);
        //}

        public ICollection<GoodsItem> SelectNew(int count)
        {
            return ExecSelect<GoodsItem>(GetProcedureString(), count);
        }
    }
}
