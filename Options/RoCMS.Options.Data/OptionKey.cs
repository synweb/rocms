//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RoCMS.Options.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class OptionKey
    {
        public OptionKey()
        {
            this.OptionValues = new HashSet<OptionValue>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool Moderated { get; set; }
    
        public virtual ICollection<OptionValue> OptionValues { get; set; }
    }
}
