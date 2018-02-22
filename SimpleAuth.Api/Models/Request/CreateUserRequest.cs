using System.Collections.Generic;

namespace SimpleAuth.Api.Models.Request
{
    public class CreateUserRequest
    {
        public string Name { get; set; }

        public string Company { get; set; }

        public string Password { get; set; }

        public UserContacts Contacts { get; set; }

        public UserOptions Options { get; set; }

        public UserAddress Address { get; set; }

        public List<UserRole> Roles { get; set; }
    }
}
