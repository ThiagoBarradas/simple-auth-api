using FluentValidation;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Utilities;

namespace SimpleAuth.Api.Validators
{
    public class CreatePasswordResetValidator : AbstractValidator<CreatePasswordResetRequest>
    {
        public CreatePasswordResetValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public class GetPasswordResetValidator : AbstractValidator<GetPasswordResetRequest>
    {
        public GetPasswordResetValidator()
        {
            RuleFor(x => x.Token).NotEmpty().Length(64);
        }
    }

    public class UsePasswordResetValidator : AbstractValidator<UsePasswordResetRequest>
    {
        public UsePasswordResetValidator()
        {
            RuleFor(x => x.Password).NotEmpty().Matches(PasswordValidator.REGEX);
        }
    }
}
