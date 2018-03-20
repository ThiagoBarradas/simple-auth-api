using System;

namespace SimpleAuth.Api.Models.Filters
{
    public class SearchAccessTokensFilters : BaseSearchFilters
    {
        public Guid? UserKey { get; set; }
    }
}
