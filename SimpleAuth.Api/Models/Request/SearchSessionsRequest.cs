using System;

namespace SimpleAuth.Api.Models.Request
{
    public class SearchSessionsRequest : BaseSearchRequest
    {
        public Guid? UserKey { get; set; }
    }
}
