using System.Net;

namespace SimpleAuth.Api.Models.Response
{
    public class BaseResponse<T>
    {
        public BaseResponse() {}

        public BaseResponse(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;

            if (statusCode == HttpStatusCode.OK ||
                statusCode == HttpStatusCode.Created ||
                statusCode == HttpStatusCode.NoContent ||
                statusCode == HttpStatusCode.Accepted)
            {
                this.IsSuccess = true;
            }
        }

        public bool IsSuccess { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public T SuccessBody { get; set; }
        
        public ErrorsResponse ErrorBody { get; set; }

        public void AddError(ErrorItemResponse error)
        {
            if (this.ErrorBody == null)
            {
                this.ErrorBody = new ErrorsResponse();
            }

            this.IsSuccess = false;
            this.ErrorBody.Errors.Add(error);
        }

        public static BaseResponse<T> Create(HttpStatusCode statusCode)
        {
            return new BaseResponse<T>
            {
                StatusCode = statusCode
            };
        }
    }
}
