using System;
using System.Collections.Generic;

namespace SimpleAuth.Api.Models
{
    public class User
    {
        public User()
        {
            this.Contacts = new UserContacts();
            this.Security = new UserSecurityWithPass();
            this.Options = new UserOptions();
            this.Roles = new List<UserRole>();
        }

        public Guid UserKey { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public UserContacts Contacts { get; set; }

        public UserOptions Options { get; set; }

        public UserAddress Address { get; set; }

        public UserSecurityWithPass Security { get; set; }

        public List<UserRole> Roles { get; set; }
    }

    public class UserContacts
    {
        public string Phone { get; set; }

        public string Email { get; set; }
    }

    public class UserOptions
    {
        public string TimeZone { get; set; }

        public string Language { get; set; }
    }

    public class UserSecurity
    {
        public bool EmailConfirmed { get; set; }

        public string EmailConfirmationToken { get; set; }
        
        public bool IsBlocked { get; set; }
    }

    public class UserSecurityWithPass: UserSecurity
    {
        public string PasswordHash { get; set; }
    }

    public class UserRole
    {
        public UserRole() { }

        public UserRole(string type)
        {
            this.Type = type;
        }

        public string Type { get; set; }

        public List<string> Keys { get; set; }
    }

    public class UserAddress
    {
        public string Country { get; set; }
    }
}
