using System;
using System.Collections.Generic;

namespace RoCMS.Options.Contract.Models
{
    public class OptionKey
    {
        public OptionKey()
        {
            OptionValues = new List<OptionValue>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Moderated { get; set; }

        public IList<OptionValue> OptionValues { get; set; } 
    }
}
