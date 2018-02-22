using MongoDB.Bson;
using MongoDB.Driver;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Repository.Interface;
using SimpleAuth.Api.Utilities.Interface;
using System;
using System.Collections.Generic;
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

        public List<AccessToken> GetAllAccessTokens(Guid userKey)
        {
            return this.Collection.Find(x => x.UserKey == userKey).ToList();
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
    }
}
