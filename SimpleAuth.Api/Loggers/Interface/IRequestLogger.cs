using Nancy;
using SimpleAuth.Api.Utilities.Interface;
using System;

namespace SimpleAuth.Api.Loggers.Interface
{
    public interface IRequestLogger
    {
        void Setup(IConfigurationUtility configurationUtility);

        void LogData(NancyContext context, Exception exception = null);
    }
}
