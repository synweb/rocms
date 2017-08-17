using System;

namespace RoCMS.Base.Models
{
    public sealed class ResultModel
    {
        #region Properties

        public object Data { get; set; }

        public bool Succeed { get; private set; }

        public string Message { get; private set; }
        public string ErrorType { get; set; }

        public static ResultModel Error
        {
            get { return new ResultModel(false); }
        }

        public static ResultModel Success
        {
            get { return ResultModel.Success; }
        }

        #endregion

        #region Constructors

        public ResultModel(bool succeed, object data = null)
        {
            Succeed = succeed;
            Data = data;
        }

        public ResultModel(bool succeed, string message, object data = null)
            : this(succeed, data)
        {
            Message = message;
        }

        public ResultModel(Exception e)
        {
            ErrorType = e.GetType().Name.Replace("Exception", "");
            Succeed = false;
        }

        #endregion
    }
}