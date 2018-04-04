using Nancy;
using Nancy.Extensions;
using Nancy.IO;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using SimpleAuth.Api.Loggers.Interface;
using SimpleAuth.Api.Utilities.Interface;
using System;
using System.IO;
using System.Linq;

namespace SimpleAuth.Api.Loggers
{
    public class SerilogLogger : IRequestLogger
    {
        public void Setup(IConfigurationUtility configurationUtility)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", "IpInfo")
                .WriteTo.Console()
                .CreateLogger();
        }

        public void LogData(NancyContext context, Exception exception = null)
        {
            if (context != null)
            {
                string template = "[{Application}] HTTP {RequestMethod} {RequestPath} from {IpAddress} responded {StatusCode} in {Elapsed} ms";
                var body = RequestStream.FromStream(context.Request.Body).AsString();
                var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? "??";

                LogContext.PushProperty("RequestBody", body);
                LogContext.PushProperty("RequestMethod", context.Request.Method);
                LogContext.PushProperty("RequestPath", context.Request.Path);
                LogContext.PushProperty("IpAddress", ip);

                if (exception != null)
                {
                    LogContext.PushProperty("Exception", exception);
                    LogContext.PushProperty("StatusCode", 500);
                    LogContext.PushProperty("Elapsed", "??");
                    Log.Error(template);
                }
                else
                {
                    var executionTime = context.Response.Headers["X-Internal-Time"] ?? "0";
                    LogContext.PushProperty("ResponseBody", this.GetResponseAsString(context));
                    LogContext.PushProperty("StatusCode", (int)context.Response.StatusCode);
                    LogContext.PushProperty("Elapsed", Convert.ToInt64(executionTime));
                    Log.Information(template);
                }
            }
        }

        private string GetResponseAsString(NancyContext context)
        {
            var stream = new MemoryStream();
            context.Response.Contents.Invoke(stream);

            stream.Position = 0;
            string responseContent = string.Empty;
            using (var reader = new StreamReader(stream))
            {
                responseContent = reader.ReadToEnd();
            }

            return responseContent;
        }
    }
}
