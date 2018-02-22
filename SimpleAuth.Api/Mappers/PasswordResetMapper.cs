using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Api.Mappers
{
    public static class PasswordResetMapper
    {
        public static GetPasswordResetResponse Map(PasswordReset passwordReset, User user)
        {
            GetPasswordResetResponse response = new GetPasswordResetResponse();

            response.UserKey = user.UserKey;
            response.Name = user.Name;
            response.Email = user.Contacts.Email;
            response.Token = passwordReset.Token;

            return response;
        } 
    }
}
