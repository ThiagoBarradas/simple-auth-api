using IpInfo.Api.Client;
using SimpleAuth.Api.Mappers;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;
using SimpleAuth.Api.Repository.Interface;
using SimpleAuth.Api.Utilities;
using SimpleAuth.Api.Utilities.Interface;
using System.Net;
using UAUtil;

namespace SimpleAuth.Api.Managers
{
    public class AuthManager : IAuthManager
    {
        private IConfigurationUtility ConfigurationUtility { get; set; }
        
        private IAuthRepository AuthRepository { get; set; }

        private IUserRepository UserRepository { get; set; }

        private IUserAgentUtility UserAgentUtility { get; set; }

        private IIpInfoApiClient IpInfoApiClient { get; set; }

        public AuthManager(IConfigurationUtility configurationUtility,
                           IIpInfoApiClient ipInfoApiClient,
                           IUserAgentUtility userAgentUtility,
                           IUserRepository userRepository,
                           IAuthRepository authRepository)
        {
            this.ConfigurationUtility = configurationUtility;
            this.AuthRepository = authRepository;
            this.UserRepository = userRepository;
            this.IpInfoApiClient = ipInfoApiClient;
            this.UserAgentUtility = userAgentUtility;
        }

        public BaseResponse<GetAccessTokenResponse> Login(LoginRequest request)
        {
            BaseResponse<GetAccessTokenResponse> response = new BaseResponse<GetAccessTokenResponse>();

            var user = this.UserRepository.GetActiveUser(
                request.Email, 
                PackUtils.HashUtility.GenerateSha256(request.Password, this.ConfigurationUtility.HashGap));

            if (user == null)
            {   
                response.StatusCode = HttpStatusCode.Unauthorized;
                return response;
            }

            response.SuccessBody = this.UserLoginCore(user, request.UserAgent, request.Ip);
            response.StatusCode = HttpStatusCode.Created;
            response.IsSuccess = true;

            return response;
        }

        private GetAccessTokenResponse UserLoginCore(User user, string userAgent, string ip)
        {
            var deviceInfo = this.UserAgentUtility.GetUserAgentDetails(userAgent);
            var ipInfo = this.IpInfoApiClient.GetIpInfo(ip);

            var accessToken = this.AuthRepository.GetAccessToken(user.UserKey, deviceInfo, ip);
            if (accessToken == null)
            {
                accessToken = AccessTokenMapper.Map(deviceInfo, ipInfo.Data, ip, user);
                accessToken.UserKey = user.UserKey;
                this.AuthRepository.CreateAccessToken(accessToken);
            }

            var userLoginResponse = AccessTokenMapper.Map(accessToken, user);

            return userLoginResponse;
        }
    }
}
