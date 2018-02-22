using SimpleAuth.Api.Models;
using System;

namespace SimpleAuth.Api.Repository.Interface
{
    public interface IPasswordResetRepository
    {
        bool CreatePasswordReset(PasswordReset passwordReset);

        PasswordReset GetPasswordReset(string token);

        bool UsePasswordReset(string token);

        bool CancelActivesPasswordResets(Guid userKey);
    }
}
