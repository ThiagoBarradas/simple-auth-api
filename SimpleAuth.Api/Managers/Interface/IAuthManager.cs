using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;

namespace SimpleAuth.Api.Managers
{
    public interface IAuthManager
    {
        BaseResponse<GetAccessTokenResponse> Login(LoginRequest request);
    }
}
