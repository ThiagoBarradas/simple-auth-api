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

    public class SearchSessionsValidator : AbstractValidator<SearchSessionsRequest>
    {
        public SearchSessionsValidator()
        {
            RuleFor(x => x.PageNumber).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }

    public class PasswordValidator
    {
        public static string REGEX = RegexUtility.GetPasswordRegex(8, 256, true, true, true, true);
    }
}
