using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using SimpleAuth.Api.Managers;
using SimpleAuth.Api.Mappers;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;
using SimpleAuth.Api.Modules.Interface;
using System;
using System.Linq;
using System.Net;

namespace SimpleAuth.Api.Controller
{
    public class AuthController : BaseController
    {
        private IAuthManager AuthManager { get; set; }

        public AuthController(IAuthManager authManager, ISecurityModule securityModule)
            : base(securityModule)
        {
            this.AuthManager = authManager;
            
            this.Post("login", args => this.Login());
            this.Get("sessions", args => this.ListSessions());
            this.Get("sessions/current", args => this.GetCurrentSession());
            this.Delete("logout", args => this.Logout());
            this.Get("logout/all", args => this.LogoutAllExceptCurrentSession());
        }

        public object Login()
        {
            var request = this.Bind<LoginRequest>();

            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            request.UserAgent = this.Request.Headers.UserAgent;
            request.Ip = this.Request.Headers["X-Forwarded-For"].FirstOrDefault() 
                         ?? this.Request.UserHostAddress;

            var response = this.AuthManager.Login(request);

            return this.CreateResponse(response);
        }

        public object ListSessions()
        {
            throw new NotImplementedException();
            //var request = this.Bind<UpdateUserRequest>();
            //
            //var validation = this.Validate(request);
            //if (validation.IsValid == false)
            //{
            //    return this.CreateBadRequestResponse(validation);
            //}
            //
            //var response = this.UserManager.UpdateUser(request);
            //
            //return this.CreateResponse(response);
        }

        public object GetCurrentSession()
        {
            if (!Authorize()) return Unauthorized();

            var accessToken = this.SecurityModule.AccessToken.Token;

            var response = new Models.Response.BaseResponse<GetAccessTokenResponse>();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.IsSuccess = true;
            response.SuccessBody = AccessTokenMapper.Map(this.SecurityModule.AccessToken, this.SecurityModule.User);

            return this.CreateResponse(response);
        }

        public object Logout()
        {
            if (!Authorize()) return Unauthorized();

            var response = this.AuthManager.Logout(this.SecurityModule.AccessToken.Token);

            return this.CreateResponse(response);
        }
        
        public object LogoutAllExceptCurrentSession()
        {
            if (!Authorize()) return Unauthorized();
            
            var response = this.AuthManager.LogoutAllExcept(this.SecurityModule.User.UserKey, this.SecurityModule.AccessToken.Token);

            return this.CreateResponse(response);
        }
    }
}
