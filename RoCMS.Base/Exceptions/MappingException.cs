using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Base.Exceptions
{
    public class MappingException: Exception
    {
        public Type InType { get; set; }
        public Type OutType { get; set; }


        public MappingException(Type inType, Type outType, string message):base(message)
        {
            InType = inType;
            OutType = outType;
        }
    }
}
