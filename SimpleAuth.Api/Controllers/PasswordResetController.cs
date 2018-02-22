using Nancy.ModelBinding;
using Nancy.Validation;
using SimpleAuth.Api.Managers;
using SimpleAuth.Api.Models.Request;

namespace SimpleAuth.Api.Controller
{
    public class PasswordResetController : BaseController
    {
        private IPasswordResetManager PasswordResetManager { get; set; }

        public PasswordResetController(IPasswordResetManager passwordResetManager) 
        {
            this.PasswordResetManager = passwordResetManager;

            this.Post("password-resets/{email}", args => this.Create());
            this.Get("password-resets/{token}", args => this.Get());
            this.Put("password-resets/{token}", args => this.Use());
        }

        public object Create()
        {
            var request = this.Bind<CreatePasswordResetRequest>();
            
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
            var request = this.Bind<GetPasswordResetRequest>();

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
            var request = this.Bind<UsePasswordResetRequest>();

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
