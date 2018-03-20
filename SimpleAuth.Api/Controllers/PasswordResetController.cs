﻿using Nancy.ModelBinding;
using Nancy.Validation;
using SimpleAuth.Api.Managers;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Modules.Interface;
using SimpleAuth.Api.Utilities;

namespace SimpleAuth.Api.Controller
{
    public class PasswordResetController : BaseController
    {
        private IPasswordResetManager PasswordResetManager { get; set; }

        public PasswordResetController(IPasswordResetManager passwordResetManager, ISecurityModule securityModule)
            : base(securityModule)
        {
            this.PasswordResetManager = passwordResetManager;

            this.Post("password-resets/{email}", args => this.Create());
            this.Get("password-resets/{token}", args => this.Get());
            this.Put("password-resets/{token}", args => this.Use());
        }

        public object Create()
        {
            var request = this.BindFromAll<CreatePasswordResetRequest>();
            
            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            var response = this.PasswordResetManager.CreatePasswordReset(request);

            return this.CreateResponse(response);
        }

        public object Get()
        {
            var request = this.BindFromAll<GetPasswordResetRequest>();

            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            var response = this.PasswordResetManager.GetPasswordReset(request);

            return this.CreateResponse(response);
        }

        public object Use()
        {
            var request = this.BindFromAll<UsePasswordResetRequest>();

            var validation = this.Validate(request);
            if (validation.IsValid == false)
            {
                return this.CreateBadRequestResponse(validation);
            }

            var response = this.PasswordResetManager.UsePasswordReset(request);

            return this.CreateResponse(response);
        }
    }
}
