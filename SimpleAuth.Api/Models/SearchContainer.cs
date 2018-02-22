using System.Collections.Generic;

namespace SimpleAuth.Api.Models
{
    public class SearchContainer<T>
    {
        public List<T> Items { get; set; }

        public long Total { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
