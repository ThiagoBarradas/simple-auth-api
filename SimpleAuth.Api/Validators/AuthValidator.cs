using FluentValidation;
using PackUtils;
using SimpleAuth.Api.Models.Request;

namespace SimpleAuth.Api.Validators
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        

        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().Matches(PasswordValidator.REGEX);
        }
    }

    public class PasswordValidator
    {
        public static string REGEX = RegexUtility.GetPasswordRegex(8, 256, true, true, true, true);
    }
}
