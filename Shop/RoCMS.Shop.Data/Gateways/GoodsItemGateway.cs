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
            var param = new GoodsFilterParam()
            {
                ActionIds=filter.ActionIds,
                Articles=filter.Articles,
                CategoryIds=filter.CategoryIds,
                WithSubcategories=filter.WithSubcategories,
                Countries=filter.Countries,
                ManufacturerIds=filter.ManufacturerIds,
                SearchQuery=filter.SearchPattern,
                FulltextSearchQuery = GetFulltextSearchQuery(filter.SearchPattern),
                SortBy = sortBy,
                SpecIds = filter.SpecIds,
                PackIds = filter.PackIds,
                SupplierIds = filter.SupplierIds,
                SortOrder = sortOrder,
                StartIndex = startIndex,
                TotalCount=total,
                Count = count
            };
            ICollection<int> ids = ExecSelect<int>("[Shop].[Goods_Filter]", param);
            totalCount = param.TotalCount;
            return ids;
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

        public ICollection<IdNamePair<int>> GetManufacturers(ICollection<int> goodsIds)
        {
            return ExecSelect<IdNamePair<int>>("[Shop].[Goods_GetManufacturers]", goodsIds);
        }

        public ICollection<IdNamePair<int>> GetCountries(ICollection<int> goodsIds)
        {
            return ExecSelect<IdNamePair<int>>("[Shop].[Goods_GetCountries]", goodsIds);
        }

        public ICollection<IdNamePair<int>> GetPacks(ICollection<int> goodsIds)
        {
            return ExecSelect<IdNamePair<int>>("[Shop].[Goods_GetPacks]", goodsIds);
        }

        public ICollection<KeyValuePair<int, string>> GetSpecs(ICollection<int> goodsIds)
        {
            var res = ExecSelect<GetSpecsResult>("[Shop].[Goods_GetSpecs]", goodsIds);
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

        public bool ExistsByRelativeUrl(string relativeUrl)
        {
            return Exec<bool>(GetProcedureString(), relativeUrl);
        }

        public GoodsItem SelectOneByRelativeUrl(string relativeUrl)
        {
            return Exec<GoodsItem>(GetProcedureString(), relativeUrl);
        }

        public ICollection<GoodsItem> SelectNew(int count)
        {
            return ExecSelect<GoodsItem>(GetProcedureString(), count);
        }
    }
}
