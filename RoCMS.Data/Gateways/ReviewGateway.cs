using System;
using System.Collections.Generic;
using RoCMS.Base.Data;
using RoCMS.Data.Models;

namespace RoCMS.Data.Gateways
{
    public class ReviewGateway : BaseGateway
    {
        public int Insert(Review review)
        {
            return Exec<int>("[dbo].[Review_Insert]", review);
        }

        public void Delete(int reviewId)
        {
            Exec("[dbo].[Review_Delete]", reviewId);
        }

        public IEnumerable<Review> Select(int startIndex, int countOnPage, out int total, bool onlyModerated)
        {
            var res = new List<Review>();
            var db = GetDb();
            using (var cmd = GetCommand("[dbo].[Review_Select]", db))
            {
                FillParams(new { start = startIndex, count = countOnPage, total = (int?)null , onlyModerated}, cmd, db);
                db.SetParameterValue(cmd, "Total", null);

                using (var reader = ExecuteReader(db, cmd))
                {
                    while (reader.Read())
                    {
                        res.Add(ReadRecord<Review>(reader));
                    }
                }
                total = Convert.ToInt32(db.GetParameterValue(cmd, "Total"));
            }
            return res;
        }

        public Review SelectOne(int reviewId)
        { 
            return Exec<Review>("[dbo].[Review_SelectOne]", reviewId);
        }

        public void Update(Review review)
        {
            Exec("[dbo].[Review_Update]", review);
        }
    }
}
