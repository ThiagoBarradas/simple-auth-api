using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;

namespace SimpleAuth.Api.Managers
{
    public interface IPasswordResetManager
    {
        BaseResponse<GetPasswordResetResponse> CreatePasswordReset(CreatePasswordResetRequest request);

        BaseResponse<GetPasswordResetResponse> GetPasswordReset(GetPasswordResetRequest request);

        BaseResponse<object> UsePasswordReset(UsePasswordResetRequest request);
    }
}
