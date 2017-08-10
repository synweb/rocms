using System;
using NLog;
using RoCMS.Web.Contract.Services;

namespace RoCMS.Logging
{
    public class LogService : ILogService
    {
        private readonly Logger _logger;

        public LogService()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void TraceMessage(string message)
        {
            _logger.Trace(message + Environment.NewLine + Environment.NewLine);
            
        }

        public Guid LogError(Exception e)
        {
            Guid code = Guid.NewGuid();
            _logger.Error("{0}" + Environment.NewLine + Environment.NewLine, new Exception(String.Format("ErrorCode: {0}", code), e));
            return code;
        }

        public Guid LogError(Exception e, string message)
        {
            Guid code = Guid.NewGuid();
            _logger.ErrorException(message + Environment.NewLine + Environment.NewLine, new Exception(String.Format("ErrorCode: {0}", code), e));
            return code;
        }

        public Guid LogAndEmailError(Exception e, string message)
        {
            Guid code = Guid.NewGuid();
            _logger.FatalException(message + Environment.NewLine + Environment.NewLine, new Exception(String.Format("ErrorCode: {0}", code), e));
            return code;
        }
    }
}
