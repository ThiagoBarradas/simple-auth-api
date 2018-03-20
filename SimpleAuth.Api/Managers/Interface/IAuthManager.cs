using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;
using System;

namespace SimpleAuth.Api.Managers
{
    public interface IAuthManager
    {
        BaseResponse<GetAccessTokenResponse> Login(LoginRequest request);

        BaseResponse<SearchResponse<GetAccessTokenResponse>> ListSessions(SearchSessionsRequest request, User user);

        BaseResponse<object> Logout(string token);

        BaseResponse<object> LogoutAllExcept(Guid userKey, string token);
    }
}
