using Nancy;
using Nancy.Validation;
using PackUtils;
using SimpleAuth.Api.Models.Response;
using SimpleAuth.Api.Modules.Interface;

namespace SimpleAuth.Api.Controller
{
    public abstract class BaseController : NancyModule
    {
        protected ISecurityModule SecurityModule { get; set; }

        public BaseController(ISecurityModule securityModule)
        {
            this.SecurityModule = securityModule;
        }

        protected object CreateResponse<T>(BaseResponse<T> response)
        {
            return this.CreateJsonResponse(response);
        }

        protected object CreateBadRequestResponse(ModelValidationResult validation)
        {
            BaseResponse<object> response = new BaseResponse<object>
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                IsSuccess = false
            };

            foreach (var error in validation.Errors)
            {
                foreach (var errorMessage in error.Value)
                {
                    response.AddError(new ErrorItemResponse(errorMessage, error.Key));
                }
            }

            return this.CreateJsonResponse(response);
        }

        private object CreateJsonResponse<T>(BaseResponse<T> response)
        {
            HttpStatusCode statusCode = response.StatusCode.ConvertToEnum<HttpStatusCode>();
            Nancy.Response httpResponse = null;

            if (response.IsSuccess == true)
            {
                httpResponse = Response.AsJson(response.SuccessBody, statusCode);
            }
            else
            {
                httpResponse = Response.AsJson(response.ErrorBody, statusCode);
            }

            httpResponse.ContentType = "application/json";

            return httpResponse;
        }

        public object Unauthorized()
        {
            var statusCode = System.Net.HttpStatusCode.Unauthorized;
            return this.CreateJsonResponse(BaseResponse<object>.Create(statusCode));
        }

        public bool Authorize()
        {
            return this.SecurityModule.Authorize(this);
        }
    }
}
