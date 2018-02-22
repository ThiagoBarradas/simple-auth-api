using System;
using System.Collections.Generic;

namespace SimpleAuth.Api.Models.Response
{
    public class GetUserResponse
    {
        public Guid UserKey { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public UserContacts Contacts { get; set; }

        public UserOptions Options { get; set; }

        public UserAddress Address { get; set; }

        public UserSecurity Security { get; set; }

        public List<UserRole> Roles { get; set; }
    }
}
