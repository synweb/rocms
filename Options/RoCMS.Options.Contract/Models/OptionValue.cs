using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Options.Contract.Models
{
    public class OptionValue
    {
        public int Id { get; set; }

        public int OptionKeyId { get; set; }
        public string Value { get; set; }

        public DateTime CreationDate { get; set; }

        public bool Moderated { get; set; }
    }
}
