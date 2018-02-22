using Nancy.ModelBinding;
using Nancy.Validation;
using SimpleAuth.Api.Managers;
using SimpleAuth.Api.Models.Request;
using System;
using System.Linq;

namespace SimpleAuth.Api.Controller
{
    public class AuthController : BaseController
    {
        private IAuthManager AuthManager { get; set; }

        public AuthController(IAuthManager authManager) 
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

        public object Logout()
        {
            throw new NotImplementedException();
            //var request = this.Bind<GetUserRequest>();
            //
            //var validation = this.Validate(request);
            //if (validation.IsValid == false)
            //{
            //    return this.CreateBadRequestResponse(validation);
            //}
            //
            //var response = this.UserManager.GetUser(request);
            //
            //return this.CreateResponse(response);
        }
        
        public object LogoutAllExceptCurrentSession()
        {
            throw new NotImplementedException();
            //var request = this.Bind<SearchUsersRequest>();
            //
            //var validation = this.Validate(request);
            //if (validation.IsValid == false)
            //{
            //    return this.CreateBadRequestResponse(validation);
            //}
            //
            //var response = this.UserManager.ListUsers(request);
            //
            //return this.CreateResponse(response);
        }
    }
}
