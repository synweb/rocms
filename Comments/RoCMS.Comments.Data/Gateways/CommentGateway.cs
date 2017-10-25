using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using RoCMS.Base.Data;
using RoCMS.Base.Models;
using RoCMS.Comments.Data.Models;

namespace RoCMS.Comments.Data.Gateways
{
    public class CommentGateway : BaseGateway
    {
        public int Insert(Comment comment)
        {
            return Exec<int>("[Comments].[Comment_Insert]", comment);
            //Database db = GetDb();
            //using (DbCommand cmd = GetCommand("[Comments].[Comment_Insert]", db))
            //{
            //    db.SetParameterValue(cmd, "ParentCommentId", comment.ParentCommentId);
            //    db.SetParameterValue(cmd, "HeartId", comment.HeartId);
            //    db.SetParameterValue(cmd, "Text", comment.Text);
            //    db.SetParameterValue(cmd, "Moderated", comment.Moderated);
            //    db.SetParameterValue(cmd, "AuthorId", comment.AuthorId);
            //    db.SetParameterValue(cmd, "Name", comment.Name);
            //    db.SetParameterValue(cmd, "Url", comment.Url);
            //    db.SetParameterValue(cmd, "Email", comment.Email);

            //    int id = Convert.ToInt32(ExecuteScalar(db, cmd));
            //    return id;
            //}
        }

        public void Delete(int commentId)
        {
            Exec("[Comments].[Comment_Delete]", commentId);
            //Database db = GetDb();
            //using (DbCommand cmd = GetCommand("[Comments].[Comment_Delete]", db))
            //{
            //    db.SetParameterValue(cmd, "CommentId", commentId);
            //    ExecuteNonQuery(db, cmd);
            //}
        }

        public void UpdateText(int commentId, string text)
        {
            Exec("[Comments].[Comment_UpdateText]", new { commentId, text });


            //Database db = GetDb();
            //using (DbCommand cmd = GetCommand("[Comments].[Comment_UpdateText]", db))
            //{
            //    db.SetParameterValue(cmd, "CommentId", commentId);
            //    db.SetParameterValue(cmd, "Text", text);
            //    ExecuteNonQuery(db, cmd);
            //}
        }

        public void UpdateModerated(int commentId, bool moderated)
        {
            Exec("[Comments].[Comment_UpdateModerated]", new { commentId, moderated});

            //Database db = GetDb();
            //using (DbCommand cmd = GetCommand("[Comments].[Comment_UpdateModerated]", db))
            //{
            //    db.SetParameterValue(cmd, "CommentId", commentId);
            //    db.SetParameterValue(cmd, "Moderated", moderated);
            //    ExecuteNonQuery(db, cmd);
            //}
        }

        public ICollection<Comment> SelectByHeart(int heartId, bool? moderated)
        {
            return ExecSelect<Comment>("[Comments].[Comment_SelectByHeart]", new { heartId, moderated });

            //IList<Comment> result = new List<Comment>();
            //Database db = GetDb();
            //using (DbCommand cmd = GetCommand("[Comments].[Comment_SelectByTopic]", db))
            //{
            //    db.SetParameterValue(cmd, "HeartId", heartId);
            //    if (onlyModerated)
            //    {
            //        db.SetParameterValue(cmd, "Moderated", true);
            //    }

            //    using (var reader = ExecuteReader(db, cmd))
            //    {
            //        while (reader.Read())
            //        {
            //            var transport = FillCommentFromReader(reader);
            //            result.Add(transport);
            //        }
            //    }
            //}
            //return result;
        }

        public ICollection<Comment> SelectByAuthor(int authorId, bool? moderated)
        {
            return ExecSelect<Comment>("[Comments].[Comment_SelectByAuthor]", new { authorId, moderated });

            //IList<Comment> result = new List<Comment>();
            //Database db = GetDb();
            //using (DbCommand cmd = GetCommand("[Comments].[Comment_SelectByTopic]", db))
            //{
            //    db.SetParameterValue(cmd, "AuthorId", authorId);
            //    if (onlyModerated)
            //    {
            //        db.SetParameterValue(cmd, "Moderated", true);
            //    }

            //    using (var reader = ExecuteReader(db, cmd))
            //    {
            //        while (reader.Read())
            //        {
            //            var transport = FillCommentFromReader(reader);
            //            result.Add(transport);
            //        }
            //    }
            //}
            //return result;
        }

        //private Comment FillCommentFromReader(IDataReader reader)
        //{
        //    Comment result = new Comment()
        //    {
        //        CommentId = Convert.ToInt32(reader["CommentId"]),
        //        AuthorId = reader["AuthorId"] == DBNull.Value ? (int?) null : Convert.ToInt32(reader["AuthorId"]),
        //        HeartId = Convert.ToInt32(reader["HeartId"]),
        //        ParentCommentId =
        //            reader["ParentCommentId"] == DBNull.Value ? (int?) null : Convert.ToInt32(reader["ParentCommentId"]),
        //        Moderated = Convert.ToBoolean(reader["Moderated"]),
        //        Text = Convert.ToString(reader["Text"]),
        //        Url = Convert.ToString(reader["Url"]),
        //        Email = Convert.ToString(reader["Email"]),
        //        Name = Convert.ToString(reader["Name"]),
        //        CreationDate = Convert.ToDateTime(reader["CreationDate"]),
        //        Deleted = Convert.ToBoolean(reader["Deleted"])
        //    };
        //    return result; 
        //}

        public Comment SelectOne(int commentId)
        {
            //Database db = GetDb();
            //using (DbCommand cmd = GetCommand("[Comments].[Comment_SelectOne]", db))
            //{
            //    db.SetParameterValue(cmd, "CommentId", commentId);

            //    using (var reader = ExecuteReader(db, cmd))
            //    {
            //        if (reader.Read())
            //        {
            //            return FillCommentFromReader(reader);
            //        }
            //        throw new RowNotInTableException();
            //    }
            //}

            return Exec<Comment>("[Comments].[Comment_SelectOne]", commentId);
        }


        public int SelectCount(int heartId)
        {

            return Exec<int>("[Comments].[Comment_SelectCount]", heartId);

            //Database db = GetDb();
            //using (DbCommand cmd = GetCommand("[Comments].[Comment_SelectCount]", db))
            //{
            //    db.SetParameterValue(cmd, "HeartId", heartId);
            //    int id = Convert.ToInt32(ExecuteScalar(db, cmd));
            //    return id;
            //}
        }

        public ICollection<int> SelectHeartIds(int startIndex, int count, out int totalCount)
        {
            var param = new CommentFilterParam()
            {
                StartIndex = startIndex,
                TotalCount = 0,
                Count = count
            };
            ICollection<int> ids = ExecSelect<int>("[Comments].[Comment_SelectHeartIds]", param);
            totalCount = param.TotalCount;
            return ids;
        }
        private class CommentFilterParam
        {
            public int StartIndex { get; set; }
            public int TotalCount { get; set; }
            public int Count { get; set; }
        }
    }


}
