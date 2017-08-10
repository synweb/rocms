using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Base.Exceptions
{
    public class UserExistsException:Exception
    {
        public UserExistsException(string msg):base(msg)
        {
        }
    }
}
