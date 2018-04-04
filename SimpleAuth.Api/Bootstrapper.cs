using IpInfo.Api.Client;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ErrorHandling;
using Nancy.TinyIoc;
using SimpleAuth.Api.Handlers;
using SimpleAuth.Api.Loggers;
using SimpleAuth.Api.Loggers.Interface;
using SimpleAuth.Api.Managers;
using SimpleAuth.Api.Modules;
using SimpleAuth.Api.Modules.Interface;
using SimpleAuth.Api.Repository;
using SimpleAuth.Api.Repository.Interface;
using SimpleAuth.Api.Utilities;
using SimpleAuth.Api.Utilities.Interface;
using System.Diagnostics;
using UAUtil;

namespace SimpleAuth.Api
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            this.AddStopwatch(pipelines);
            this.EnableCors(pipelines);
            this.EnableCSRF(pipelines);
            this.InitLogger(pipelines, container);
            this.InitializeRepository();
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            // Utilities
            container.Register<IConfigurationRoot>(Startup.Configuration);
            container.Register<IConfigurationUtility, ConfigurationUtility>().AsSingleton();
            container.Register<IUserAgentUtility, UserAgentUtility>().AsSingleton();

            // Loggers
            container.Register<IExceptionLogger, RollbarLogger>().AsSingleton();
            container.Register<IRequestLogger, SerilogLogger>().AsSingleton();

            // Others
            container.Register(PackUtils.JsonUtility.CamelCaseJsonSerializer);
            container.Register<IStatusCodeHandler, StatusCodeHandler>().AsSingleton();
            
            base.ConfigureApplicationContainer(container);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            // Utils
            var ipInfoClient = new IpInfoApiClient(Startup.Configuration["IpInfoApiUrl"]);
            container.Register<IIpInfoApiClient>(ipInfoClient);

            // Modules
            container.Register<ISecurityModule, SecurityModule>().AsSingleton();

            // Manager
            container.Register<IPasswordResetManager, PasswordResetManager>().AsSingleton();
            container.Register<IUserManager, UserManager>().AsSingleton();
            container.Register<IAuthManager, AuthManager>().AsSingleton();

            // Repository
            container.Register<IPasswordResetRepository, PasswordResetRepository>().AsSingleton();
            container.Register<IUserRepository, UserRepository>().AsSingleton();

            base.ConfigureRequestContainer(container, context);
        }

        private void EnableCors(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToStartOfPipeline((context) =>
            {
                context.Response
                       .WithHeader("Access-Control-Allow-Origin", "*")
                       .WithHeader("Access-Control-Allow-Methods", "GET,HEAD,OPTIONS,POST,PUT,PATCH,DELETE")
                       .WithHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
            });
        }

        private void AddStopwatch(IPipelines pipelines)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline((context) =>
            {
                var stopwatch = Stopwatch.StartNew();
                context.Items.Add("Stopwatch", stopwatch);
                return null;
            });

            pipelines.AfterRequest.AddItemToStartOfPipeline((context) =>
            {
                context.Items.TryGetValue("Stopwatch", out object objStopwatch);
                if (objStopwatch != null)
                {
                    Stopwatch stopwatch = (Stopwatch)objStopwatch;
                    stopwatch.Stop();
                    context.Response.Headers.Add("X-Internal-Time", stopwatch.ElapsedMilliseconds.ToString());
                }
            });
        }

        private void EnableCSRF(IPipelines pipelines)
        {
            Nancy.Security.Csrf.Enable(pipelines);
        }

        private void InitializeRepository()
        {
            BaseRepository<object>.InitializeRepository();
        }

        private void InitLogger(IPipelines pipelines, TinyIoCContainer container)
        {
            container.Resolve<IRequestLogger>().Setup(container.Resolve<IConfigurationUtility>());

            pipelines.OnError.AddItemToStartOfPipeline((context, exception) =>
            {
                container.Resolve<IExceptionLogger>().LogCritical(exception);
                container.Resolve<IRequestLogger>().LogData(context, exception);
                return null;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline((context) =>
            {
                container.Resolve<IRequestLogger>().LogData(context);
            });

            pipelines.OnError.AddItemToStartOfPipeline((context, exception) =>
            {
                container.Resolve<IExceptionLogger>().LogCritical(exception);
                return null;
            });
        }
    }
}