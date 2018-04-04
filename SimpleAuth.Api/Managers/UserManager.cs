using SimpleAuth.Api.Mappers;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;
using SimpleAuth.Api.Repository.Interface;
using SimpleAuth.Api.Utilities.Interface;
using System.Net;

namespace SimpleAuth.Api.Managers
{
    public class UserManager : IUserManager
    {
        private IConfigurationUtility ConfigurationUtility { get; set; }
        
        private IUserRepository UserRepository { get; set; }

        public UserManager(IConfigurationUtility configurationUtility, 
                           IUserRepository userRepository)
        {
            this.ConfigurationUtility = configurationUtility;
            this.UserRepository = userRepository;
        }

        public BaseResponse<GetUserResponse> CreateUser(CreateUserRequest request)
        {
            BaseResponse<GetUserResponse> response = new BaseResponse<GetUserResponse>();

            var user = this.UserRepository.GetUser(request.Contacts.Email);

            if (user == null)
            {
                user = UserMapper.Map(request, this.ConfigurationUtility);
                this.UserRepository.CreateOrUpdateUser(user);
                response.SuccessBody = UserMapper.Map(user);
                response.StatusCode = HttpStatusCode.Created;
                response.IsSuccess = true;
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.AddError(new ErrorItemResponse("email", "Email already registered"));
            }

            return response;
        }

        public BaseResponse<GetUserResponse> UpdateUser(UpdateUserRequest request)
        {
            BaseResponse<GetUserResponse> response = new BaseResponse<GetUserResponse>();

            var user = this.UserRepository.GetUser(request.UserKey);

            if (user != null)
            {
                user = UserMapper.Map(user, request, this.ConfigurationUtility);
                this.UserRepository.CreateOrUpdateUser(user);

                response.StatusCode = HttpStatusCode.OK;
                response.SuccessBody = UserMapper.Map(user);
                response.IsSuccess = true;
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        public BaseResponse<GetUserResponse> GetUser(GetUserRequest request)
        {
            BaseResponse<GetUserResponse> response = new BaseResponse<GetUserResponse>();

            var user = this.UserRepository.GetUser(request.UserKey);

            if (user != null)
            {
                GetUserResponse userResponse = UserMapper.Map(user);
                response.SuccessBody = userResponse;
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        public BaseResponse<object> ConfirmUserEmail(ConfirmUserEmailRequest request)
        {
            BaseResponse<object> response = new BaseResponse<object>();

            var result = this.UserRepository.ConfirmUserEmail(request.EmailConfirmationToken);

            response.StatusCode = (result == true) 
                    ? HttpStatusCode.OK 
                    : HttpStatusCode.NotFound;

            return response;
        }

        public BaseResponse<SearchResponse<GetUserResponse>> ListUsers(SearchUsersRequest request)
        {
            BaseResponse<SearchResponse<GetUserResponse>> response = new BaseResponse<SearchResponse<GetUserResponse>>();

            var filters = UserMapper.Map(request);
            var users = this.UserRepository.ListUsers(filters);

            SearchResponse<GetUserResponse> usersResponse = UserMapper.Map(users);
            response.SuccessBody = usersResponse;
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;

            return response;
        }

        public BaseResponse<object> IsEmailAvailable(IsEmailAvailableRequest request)
        {
            BaseResponse<object> response = new BaseResponse<object>();

            var result = this.UserRepository.ExistsEmail(request.Email);

            response.StatusCode = (result == true)
                    ? HttpStatusCode.OK
                    : HttpStatusCode.NotFound;

            return response;
        }
    }
}
