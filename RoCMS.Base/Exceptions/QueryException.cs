using System;

namespace RoCMS.Base.Exceptions
{
    internal class QueryException : Exception
    {
        public QueryException(string s)
            : base(s)
        { }
    }
}
