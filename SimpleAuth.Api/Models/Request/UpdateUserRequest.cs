using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Api.Models.Request
{
    public class UpdateUserRequest : CreateUserRequest
    {
        public Guid UserKey { get; set; }
    }
}