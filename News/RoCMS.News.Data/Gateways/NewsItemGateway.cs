using System;
using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.News.Data.Models;
using NewsItem = RoCMS.News.Data.Models.NewsItem;

namespace RoCMS.News.Data.Gateways
{
    public class NewsItemGateway : BasicGateway<Models.NewsItem>
    {
        protected override string DefaultScheme => "News";

        public ICollection<int> SelectRelated(int newsId, bool withSubnews, int count, bool onlyPosted = false)
        {
            return ExecSelect<int>(GetProcedureString(), new { newsId, withSubnews, count, onlyPosted });
        }
        
        public ICollection<int> SelectIds(bool onlyPosted = false)
        {
            return ExecSelect<int>(GetProcedureString(), onlyPosted);
        }

        public ICollection<int> SelectIdsByTagName(string tagName, bool onlyPosted = false)
        {
            return ExecSelect<int>(GetProcedureString(), new{tagName, onlyPosted});
        }

        public ICollection<NewsItem> SelectFilteredPage(FinalNewsFilter dataFilter, int pageNumber, int pageSize, out int totalCount)
        {
            var res = new List<NewsItem>();
            var db = GetDb();
            using (var cmd = GetCommand(GetProcedureString(), db))
            {
                FillParams(new
                {
                    pageNumber,
                    pageSize,
                    dataFilter.NewsIds,
                    dataFilter.BlogId,
                    dataFilter.OnlyFutureEventDate,
                    dataFilter.RecordTypes,
                    dataFilter.SortBy,
                    dataFilter.SortOrder,
                    dataFilter.OnlyPosted,
                    totalCount = (int?)null
                }, cmd, db);
                db.SetParameterValue(cmd, "TotalCount", null);

                using (var reader = ExecuteReader(db, cmd))
                {
                    while (reader.Read())
                    {
                        res.Add(ReadRecord<NewsItem>(reader));
                    }
                }
                totalCount = Convert.ToInt32(db.GetParameterValue(cmd, "TotalCount"));
            }
            return res;
        }

        public void IncreaseViewCount(int newsId)
        {
            Exec(GetProcedureString(), newsId);
        }
    }
}
