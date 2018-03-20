using MongoDB.Bson;
using MongoDB.Driver;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Filters;
using SimpleAuth.Api.Repository.Interface;
using SimpleAuth.Api.Repository.QueryBuilders;
using SimpleAuth.Api.Utilities.Interface;
using System;
using UAUtil.Models;

namespace SimpleAuth.Api.Repository
{
    public class AuthRepository : BaseRepository<AccessToken>, IAuthRepository
    {
        public const string COLLECTION_NAME = nameof(AccessToken);

        public AuthRepository(IConfigurationUtility configurationUtility)
            : base(configurationUtility, AuthRepository.COLLECTION_NAME) { }

        public bool CreateAccessToken(AccessToken accessToken)
        {
            return this.Collection.ReplaceOne(new BsonDocument("_id", accessToken.Token),
                accessToken,
                new UpdateOptions() { IsUpsert = true }
                ).IsAcknowledged;
        }

        public bool DeleteAccessToken(string token)
        {
            return this.Collection.DeleteOne(new BsonDocument("_id", token)).IsAcknowledged;
        }

        public bool DeleteAllAccessTokens(Guid userKey, string exceptToken)
        {
            return this.Collection.DeleteMany(at => at.UserKey == userKey && at.Token != exceptToken).IsAcknowledged;
        }

        public AccessToken GetAccessToken(string token)
        {
            return this.Collection.Find(u => u.Token == token).FirstOrDefault();
        }

        public SearchContainer<AccessToken> GetAllAccessTokens(SearchAccessTokensFilters filters)
        {
            FilterDefinition<AccessToken> filter = this.BuildAccessTokensQuery(filters);
           
            var result = this.Collection.Find(filter)
                .WithSorting(filters)
                .WithPaging(filters).ToList();

            return new SearchContainer<AccessToken>()
            {
                Items = result,
                Total = result.Count,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize
            };
        }

        public AccessToken GetAccessToken(Guid userKey, UserAgentDetails deviceInfo, string ip)
        {
            return this.Collection.Find(at => 
                at.UserKey == userKey && 
                at.DeviceInfo.Browser == deviceInfo.Browser && 
                at.DeviceInfo.Platform == deviceInfo.Platform && 
                at.Ip == ip &&
                at.DeviceInfo.OperatingSystem == deviceInfo.OperatingSystem).FirstOrDefault();
        }

        private FilterDefinition<AccessToken> BuildAccessTokensQuery(SearchAccessTokensFilters filters)
        {
            FilterDefinition<AccessToken> filter = null;

            if (filters.UserKey != null)
            {
                filter = filter.FilterJoin(Builders<AccessToken>.Filter.Eq(x => x.UserKey, filters.UserKey));
            }

            if (filter == null) filter = Builders<AccessToken>.Filter.Empty;

            return filter;
        }
    }
}
