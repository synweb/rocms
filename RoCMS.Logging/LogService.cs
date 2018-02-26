using System;
using System.Linq;
using System.Reflection;
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
            _logger.Trace(message);
            
        }

        public Guid LogError(Exception e)
        {
            Guid code = Guid.NewGuid();
            if (e is ReflectionTypeLoadException exception)
            {
                _logger.Error(new Exception($"ErrorCode: {code}", e));
                var loaderExceptions = exception.LoaderExceptions;
                if (loaderExceptions.Any())
                {
                    _logger.Error("Loader exceptions:");
                }
                foreach (var loaderException in loaderExceptions)
                {
                    _logger.Error(loaderException);
                }
                _logger.Error(Environment.NewLine + Environment.NewLine);
            }
            else
            {
                _logger.Error("{0}" + Environment.NewLine + Environment.NewLine,
                    new Exception($"ErrorCode: {code}", e));
            }
            return code;
        }

        public Guid LogError(Exception e, string message)
        {
            Guid code = Guid.NewGuid();
            _logger.ErrorException(message + Environment.NewLine + Environment.NewLine, new Exception(
                $"ErrorCode: {code}", e));
            return code;
        }

        public Guid LogAndEmailError(Exception e, string message)
        {
            Guid code = Guid.NewGuid();
            _logger.FatalException(message + Environment.NewLine + Environment.NewLine, new Exception(
                $"ErrorCode: {code}", e));
            return code;
        }
    }
}
