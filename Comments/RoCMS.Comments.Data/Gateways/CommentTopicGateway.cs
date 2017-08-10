using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using RoCMS.Base.Data;
using RoCMS.Base.Models;
using RoCMS.Comments.Data.Models;

namespace RoCMS.Comments.Data.Gateways
{
    public class CommentTopicGateway: BaseGateway
    {
        public int Insert(CommentTopic topic)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand("[Comments].[CommentTopic_Insert]", db))
            {
                db.SetParameterValue(cmd, "TargetType", topic.TargetType);
                db.SetParameterValue(cmd, "TargetId", topic.TargetId);
                db.SetParameterValue(cmd, "TargetUrl", topic.TargetUrl);
                db.SetParameterValue(cmd, "TargetTitle", topic.TargetTitle);

                int id = Convert.ToInt32(ExecuteScalar(db, cmd));
                return id;
            }
        }

        public void Delete(int commentTopicId)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand("[Comments].[CommentTopic_Delete]", db))
            {
                db.SetParameterValue(cmd, "CommentTopicId", commentTopicId);

                ExecuteNonQuery(db, cmd);
            }
        }

        public ICollection<CommentTopic> Select(PagingFilter paging, out int totalCount, string targetType=null)
        {
            IList<CommentTopic> result = new List<CommentTopic>();
            Database db = GetDb();
            using (DbCommand cmd = GetCommand("[Comments].[CommentTopic_Select]", db))
            {
                db.SetParameterValue(cmd, "Page", paging.Page);
                db.SetParameterValue(cmd, "PageSize", paging.PageSize);
                db.SetParameterValue(cmd, "TotalCount", null);
                if (!String.IsNullOrWhiteSpace(targetType))
                {
                    db.SetParameterValue(cmd, "TargetType", targetType);                    
                }
                using (var reader = ExecuteReader(db, cmd))
                {
                    while (reader.Read())
                    {
                        result.Add(FillCommentTopicFromReader(reader));
                    }
                }
                totalCount = Convert.ToInt32(db.GetParameterValue(cmd, "TotalCount"));
            }
            return result;
        }

        private CommentTopic FillCommentTopicFromReader(IDataReader reader)
        {
            var res = new CommentTopic()
            {
                CommentTopicId = Convert.ToInt32(reader["CommentTopicId"]),
                TargetType = reader["TargetType"] == DBNull.Value? null : Convert.ToString(reader["TargetType"]),
                TargetId = reader["TargetId"] == DBNull.Value ? (int?) null : Convert.ToInt32(reader["TargetId"]),
                TargetTitle = reader["TargetTitle"] == DBNull.Value ? null : Convert.ToString(reader["TargetTitle"]),
                TargetUrl = reader["TargetUrl"] == DBNull.Value ? null : Convert.ToString(reader["TargetUrl"]),
            };
            return res;
        }

        public CommentTopic SelectOne(int id)
        {
            Database db = GetDb();
            using (DbCommand cmd = GetCommand("[Comments].[CommentTopic_SelectOne]", db))
            {
                db.SetParameterValue(cmd, "CommentTopicId", id);
                using (var reader = ExecuteReader(db, cmd))
                {
                    if (reader.Read())
                    {
                        return FillCommentTopicFromReader(reader);
                    }
                    throw new RowNotInTableException();
                }
            }
            
        }
    }
}
