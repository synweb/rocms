using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Models
{
     public class MailSendResult
    {
        public bool Success { get; set; }
        public string ErrorMsg { get; set; }
    }
}
