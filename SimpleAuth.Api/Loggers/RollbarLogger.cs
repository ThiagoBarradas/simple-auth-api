using Microsoft.Extensions.Options;
using RollbarDotNet;
using RollbarDotNet.Abstractions;
using RollbarDotNet.Builder;
using RollbarDotNet.Configuration;
using SimpleAuth.Api.Loggers.Interface;
using SimpleAuth.Api.Utilities.Interface;
using System;

namespace SimpleAuth.Api.Loggers
{
    public class RollbarLogger : IExceptionLogger
    {
        private Rollbar Rollbar { get; set; }

        public RollbarLogger(IConfigurationUtility configurationUtility)
        {
            var rollbarOptions = Options.Create(new RollbarOptions
            {
                AccessToken = configurationUtility.RollbarAccessToken,
                Environment = configurationUtility.RollbarEnvironment
            });

            this.Rollbar = new Rollbar(
                new IBuilder[] {
                    new ConfigurationBuilder(rollbarOptions),
                    new EnvironmentBuilder(new SystemDateTime()),
                    new NotifierBuilder()
                },
                new IExceptionBuilder[] {
                    new ExceptionBuilder()
                },
                new RollbarClient(rollbarOptions)
            );
        }

        public void LogCritical(Exception exception)
        {
            this.Rollbar.SendException(RollbarLevel.Critical, exception);
        }

        public void LogInfo(string message)
        {
            this.Rollbar.SendMessage(RollbarLevel.Info, message);
        }
    }
}
