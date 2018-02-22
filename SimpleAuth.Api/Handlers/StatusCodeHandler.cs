using Nancy;
using Nancy.ErrorHandling;
using Nancy.ViewEngines;
using System.Collections.Generic;

namespace SimpleAuth.Api.Handlers
{
    public class StatusCodeHandler : NancyModule, IStatusCodeHandler
    {
        private readonly IRootPathProvider RootPathProvider;
        private readonly IViewRenderer ViewRenderer;

        public StatusCodeHandler(IRootPathProvider rootPathProvider, IViewRenderer viewRenderer)
        {
            this.RootPathProvider = rootPathProvider;
            this.ViewRenderer = viewRenderer;
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            List<HttpStatusCode> statusCodesToHandle = new List<HttpStatusCode>();

            statusCodesToHandle.Add(HttpStatusCode.NotFound);
            statusCodesToHandle.Add(HttpStatusCode.InternalServerError);

            return statusCodesToHandle.Contains(statusCode);
        }

        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            context.Response = new Response()
            {
                StatusCode = statusCode
            };
        }
    }
}
