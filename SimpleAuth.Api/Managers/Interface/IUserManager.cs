using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;

namespace SimpleAuth.Api.Managers
{
    public interface IUserManager
    {
        BaseResponse<GetUserResponse> CreateUser(CreateUserRequest request);

        BaseResponse<GetUserResponse> UpdateUser(UpdateUserRequest request);

        BaseResponse<GetUserResponse> GetUser(GetUserRequest request);

        BaseResponse<SearchResponse<GetUserResponse>> ListUsers(SearchUsersRequest request);

        BaseResponse<object> IsEmailAvailable(IsEmailAvailableRequest request);

        BaseResponse<object> ConfirmUserEmail(ConfirmUserEmailRequest request);
    }
}
