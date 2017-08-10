using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Data.Models
{
    public class FormRequest
    {
        public int FormRequestId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
