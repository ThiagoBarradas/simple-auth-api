namespace SimpleAuth.Api.Models.Request
{
    public class SearchUsersRequest : BaseSearchRequest
    {
        public string Keywords { get; set; }

        public string Role { get; set; }

        public string PermissionKey { get; set; }

        public bool? IsActive { get; set; }
    }
}
