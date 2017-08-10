using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace RoCMS.Models
{
    
    public class ErrorModel
    {
        public ErrorModel(HttpStatusCode code, Guid? errorId)
        {
            Code = code;
            ErrorId = errorId;
        }

        public HttpStatusCode Code {get;set;}
        public Guid? ErrorId { get; set; }
    }
}