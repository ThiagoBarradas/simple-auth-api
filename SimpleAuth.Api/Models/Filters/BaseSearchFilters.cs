namespace SimpleAuth.Api.Models.Filters
{
    public class BaseSearchFilters
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string SortMode { get; set; }

        public string SortField { get; set; }

        public string Keywords { get; set; }

        public int Offset
        {
            get
            {
                return (this.PageNumber - 1) * this.PageSize;
            }
        }
    }
}