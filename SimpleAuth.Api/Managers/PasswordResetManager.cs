using PackUtils;
using SimpleAuth.Api.Mappers;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Enums;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;
using SimpleAuth.Api.Repository.Interface;
using SimpleAuth.Api.Utilities;
using SimpleAuth.Api.Utilities.Interface;
using System;
using System.Net;

namespace SimpleAuth.Api.Managers
{
    public class PasswordResetManager : IPasswordResetManager
    {
        private IConfigurationUtility ConfigurationUtility { get; set; }

        private IPasswordResetRepository PasswordResetRepository { get; set; }

        private IUserRepository UserRepository { get; set; }

        public PasswordResetManager(IConfigurationUtility configurationUtility, 
                                    IPasswordResetRepository passwordResetRepository,
                                    IUserRepository userRepository)
        {
            this.ConfigurationUtility = configurationUtility;
            this.PasswordResetRepository = passwordResetRepository;
            this.UserRepository = userRepository;
        }

        public BaseResponse<GetPasswordResetResponse> CreatePasswordReset(CreatePasswordResetRequest request)
        {
            BaseResponse<GetPasswordResetResponse> response = new BaseResponse<GetPasswordResetResponse>();

            var user = this.UserRepository.GetUser(request.Email);

            if (this.IsUserEligibleForRequestPasswordReset(user))
            {
                this.PasswordResetRepository.CancelActivesPasswordResets(user.UserKey);

                var passwordReset = PasswordReset.New(user.UserKey, this.ConfigurationUtility.PasswordResetExpiresInHours);
                this.PasswordResetRepository.CreatePasswordReset(passwordReset);

                response.StatusCode = HttpStatusCode.Created;
                response.SuccessBody = PasswordResetMapper.Map(passwordReset, user);
                response.IsSuccess = true;
            }
            else
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

            return response;
        }

        public BaseResponse<GetPasswordResetResponse> GetPasswordReset(GetPasswordResetRequest request)
        {
            BaseResponse<GetPasswordResetResponse> response = new BaseResponse<GetPasswordResetResponse>();

            var passwordReset = this.PasswordResetRepository.GetPasswordReset(request.Token);

            if (this.IsPasswordResetEligibleForUse(passwordReset))
            {
                var user = this.UserRepository.GetUser(passwordReset.UserKey);

                if (this.IsUserEligibleForRequestPasswordReset(user))
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.SuccessBody = PasswordResetMapper.Map(passwordReset, user);
                    response.IsSuccess = true;
                    return response;
                }
            }

            response.StatusCode = HttpStatusCode.NotFound;
            
            return response;
        }

        public BaseResponse<object> UsePasswordReset(UsePasswordResetRequest request)
        {
            BaseResponse<object> response = new BaseResponse<object>();

            var passwordReset = this.PasswordResetRepository.GetPasswordReset(request.Token);

            if (this.IsPasswordResetEligibleForUse(passwordReset))
            {
                var user = this.UserRepository.GetUser(passwordReset.UserKey);

                if (this.IsUserEligibleForRequestPasswordReset(user))
                {
                    var passwordHash = HashUtility.GenerateSha256(request.Password, this.ConfigurationUtility.HashGap);
                    this.UserRepository.UpdateUserPassword(user.UserKey, passwordHash);
                    this.PasswordResetRepository.UsePasswordReset(request.Token);

                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                    return response;
                }
            }

            response.StatusCode = HttpStatusCode.NotFound;

            return response;
        }

        private bool IsPasswordResetEligibleForUse(PasswordReset passwordReset)
        {
            return (passwordReset != null &&
                    passwordReset.Status == PasswordResetStatusEnum.Created &&
                    passwordReset.ExpirationDate >= DateTime.UtcNow);
        }

        private bool IsUserEligibleForRequestPasswordReset(User user)
        {
            return (user != null &&
                    user.Security.IsBlocked == false);
        }
    }
}
