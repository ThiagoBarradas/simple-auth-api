﻿using Nancy.ModelBinding;
using Nancy.Validation;
using SimpleAuth.Api.Managers;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Modules.Interface;
using SimpleAuth.Api.Utilities;

namespace SimpleAuth.Api.Controller
{
    public class UserController : BaseController
    {
        private IUserManager UserManager { get; set; }

        public UserController(IUserManager userManager, ISecurityModule securityModule)
            : base(securityModule)
        {
            this.UserManager = userManager;

            this.Post("users", args => this.Create());
            this.Get("users", args => this.List());
            this.Patch("users/email-activate/{emailConfirmationToken}", args => this.ConfirmEmail());
            this.Get("users/email/{email}/exists", args => this.IsEmailAvailable());
            this.Get("users/{userKey}", args => this.Get());
            this.Put("users/{userKey}", args => this.Update());
        }

        public object Create()
        {
            var request = this.BindFromAll<CreateUserRequest>();
            
            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            var response = this.UserManager.CreateUser(request);

            return this.CreateResponse(response);
        }

        public object Update()
        {
            var request = this.BindFromAll<UpdateUserRequest>();

            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            var response = this.UserManager.UpdateUser(request);

            return this.CreateResponse(response);
        }

        public object Get()
        {
            var request = this.BindFromAll<GetUserRequest>();

            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            var response = this.UserManager.GetUser(request);

            return this.CreateResponse(response);
        }
        
        public object List()
        {
            var request = this.BindFromAll<SearchUsersRequest>();

            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            var response = this.UserManager.ListUsers(request);

            return this.CreateResponse(response);
        }

        public object IsEmailAvailable()
        {
            var request = this.BindFromAll<IsEmailAvailableRequest>();

            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            var response = this.UserManager.IsEmailAvailable(request);

            return this.CreateResponse(response);
        }

        public object ConfirmEmail()
        {
            var request = this.BindFromAll<ConfirmUserEmailRequest>();

            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            var response = this.UserManager.ConfirmUserEmail(request);

            return this.CreateResponse(response);
        }
    }
}
