using System;

namespace SimpleAuth.Api.Models.Response
{
    public class GetPasswordResetResponse
    {
        public Guid UserKey { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
