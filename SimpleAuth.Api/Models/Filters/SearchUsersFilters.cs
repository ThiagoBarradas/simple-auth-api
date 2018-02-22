using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAuth.Api.Models.Filters
{
    public class SearchUsersFilters : BaseSearchFilters
    {
        public string Role { get; set; }

        public string PermissionKey { get; set; }

        public bool? IsActive { get; set; }
    }
}
