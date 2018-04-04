using System;

namespace SimpleAuth.Api.Loggers.Interface
{
    public interface IExceptionLogger
    {
        void LogInfo(string message);

        void LogCritical(Exception exception);
    }
}
