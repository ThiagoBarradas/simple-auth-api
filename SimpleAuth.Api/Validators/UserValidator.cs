using FluentValidation;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Request;
using System.Collections.Generic;

namespace SimpleAuth.Api.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 128);
            RuleFor(x => x.Company).Length(3, 32);
            RuleFor(x => x.Password).NotEmpty().Matches(PasswordValidator.REGEX);
            RuleFor(x => x.Contacts).NotNull();
            RuleFor(x => x.Contacts.Email).NotEmpty().EmailAddress();
        }
    }

    public class ListUsersValidator : AbstractValidator<SearchUsersRequest>
    {
        public ListUsersValidator()
        {
            RuleFor(x => x.PageNumber).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }

    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 128);
            RuleFor(x => x.Company).Length(3, 32);
            RuleFor(x => x.Password).Matches(PasswordValidator.REGEX);
            RuleFor(x => x.Contacts).NotNull();
            RuleFor(x => x.Contacts.Email).NotEmpty().EmailAddress();
        }
    }

    public class GetUserValidator : AbstractValidator<GetUserRequest>
    {
        public GetUserValidator()
        {
            RuleFor(x => x.UserKey).NotEmpty();
        }
    }
    
    public class GetEmailIsAvailableValidator : AbstractValidator<IsEmailAvailableRequest>
    {
        public GetEmailIsAvailableValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public class ConfirmUserEmailValidator : AbstractValidator<ConfirmUserEmailRequest>
    {
        public ConfirmUserEmailValidator()
        {
            RuleFor(x => x.EmailConfirmationToken).NotEmpty();
        }
    }

    public class UserContactsValidator : AbstractValidator<UserContacts>
    {
        public UserContactsValidator()
        {
        }
    }

    public class UserAddressValidator : AbstractValidator<UserAddress>
    {
        public UserAddressValidator()
        {
        }
    }

    public class UserRoleValidator : AbstractValidator<UserRole>
    {
        public UserRoleValidator()
        {
        }
    }

    public class UserRolesValidator : AbstractValidator<List<UserRole>>
    {
        public UserRolesValidator()
        {
        }
    }

    public class UserOptionsValidator : AbstractValidator<UserOptions>
    {
        public UserOptionsValidator()
        {
        }
    }
}
