using MongoDB.Driver;
using SimpleAuth.Api.Models.Enums;
using SimpleAuth.Api.Models.Filters;

namespace SimpleAuth.Api.Repository.QueryBuilders
{
    public static class QueryFilterExtensions
    {
        public static FilterDefinition<T> AddMongoFilters<T>(this BaseSearchFilters filters)
        {
            var builder = Builders<T>.Filter;

            FilterDefinition<T> filter = string.IsNullOrEmpty(filters.Keywords) ? builder.Empty : builder.Text(filters.Keywords);

            return filter;
        }

        public static IFindFluent<T, T> WithSorting<T>(this IFindFluent<T, T> queryResults, BaseSearchFilters filters)
        {
            var sortBuilder = Builders<T>.Sort;

            SortDefinition<T> sortFilter;

            if (filters.SortMode != null && filters.SortField != null)
            {
                if (filters.SortMode.Equals("asc", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    sortFilter = sortBuilder.Ascending(filters.SortField);
                }
                else
                {
                    sortFilter = sortBuilder.Descending(filters.SortField);
                }

                queryResults.Sort(sortFilter);
            }

            return queryResults;
        }

        public static IFindFluent<T, T> WithPaging<T>(this IFindFluent<T, T> queryResults, BaseSearchFilters filters)
        {
            queryResults.Skip(filters.Offset).Limit(filters.PageSize);
            return queryResults;
        }

        public static FilterDefinition<T> FilterJoin<T>(this FilterDefinition<T> filter, FilterDefinition<T> filterToAdd, OperatorEnum operation = OperatorEnum.And)
        {
            if (filterToAdd == null) return filter;
            if (filter == null)
            {
                filter = filterToAdd;
                return filterToAdd;
            }
            
            if (operation == OperatorEnum.And)
            {
                filter = filter & filterToAdd;
            }
            else
            {
                filter = filter | filterToAdd;
            }

            return filter;
        }
    }
}
