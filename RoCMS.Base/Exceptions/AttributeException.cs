using System;

namespace RoCMS.Base.Exceptions
{
    internal class AttributeException : Exception
    {
        public AttributeException(string s)
            : base(s)
        { }
    }
}