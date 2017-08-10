using System;

namespace RoCMS.Web.Contract.Services
{
    public interface ILogService
    {
        void TraceMessage(string message);
        Guid LogError(Exception e);
        Guid LogError(Exception e, string message);
        Guid LogAndEmailError(Exception e, string message);
    }
}
