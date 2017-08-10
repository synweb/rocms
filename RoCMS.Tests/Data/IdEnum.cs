using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Tests.Data
{
    public class IdStringEnum
    {
        public int Id { get; set; }
        public DbEnum Enum { get; set; }
    }

    public enum DbEnum
    {
        First,
        Second,
        Third,
        Fourth
    }
}
