using System.Collections.Generic;

namespace SimpleAuth.Api.Models.Response
{
    public class SearchResponse<T>
    {
        public List<T> Items { get; set; }

        public PagedList.PagedList Paging { get; set; }
    }
}
