using SimpleAuth.Api.Models.Enums;

namespace SimpleAuth.Api.Models.Request
{
    public class BaseSearchRequest
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public SortModeEnum SortMode { get; set; }

        public string SortField { get; set; }
    }
}
