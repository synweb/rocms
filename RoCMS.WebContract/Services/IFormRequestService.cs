using RoCMS.Web.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoCMS.Web.Contract.Services
{
    public interface IFormRequestService
    {
        int CreateFormRequest(FormRequest formRequest);

        FormRequest GetOneFormRequest(int formRequestId);

        IList<FormRequest> GetFormRequests();

        void UpdateFormRequest(FormRequest formRequest);

        void DeleteFormRequest(int formRequestId);

        void ProcessMessage(Message message);
        event EventHandler<FormRequest> RequestCreated;
    }
}
