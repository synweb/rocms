using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Base.Exceptions
{
    public class OAuthException: Exception
    {
        public OAuthException()
        {
            
        }

        public OAuthException(string message):base(message)
        {
            
        }
    }
}
