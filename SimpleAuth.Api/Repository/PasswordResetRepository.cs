using MongoDB.Bson;
using MongoDB.Driver;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Enums;
using SimpleAuth.Api.Repository.Interface;
using SimpleAuth.Api.Utilities.Interface;
using System;

namespace SimpleAuth.Api.Repository
{
    public class PasswordResetRepository : BaseRepository<PasswordReset>, IPasswordResetRepository
    {
        public const string COLLECTION_NAME = nameof(PasswordReset);

        public PasswordResetRepository(IConfigurationUtility configurationUtility) 
            : base(configurationUtility, PasswordResetRepository.COLLECTION_NAME) { }

        public bool CreatePasswordReset(PasswordReset passwordReset)
        {
            return this.Collection.ReplaceOne(
                new BsonDocument("_id", passwordReset.Token),
                passwordReset,
                new UpdateOptions() { IsUpsert = true }
            ).IsAcknowledged;
        }

        public PasswordReset GetPasswordReset(string token)
        {
            return this.Collection.Find(new BsonDocument("_id", token)).FirstOrDefault();
        }

        public bool UsePasswordReset(string token)
        {
            var filter = Builders<PasswordReset>.Filter.Eq(x => x.Token, token);
            var update = Builders<PasswordReset>.Update.Set(x => x.Status, PasswordResetStatusEnum.Used);

            return this.Collection.UpdateOne(filter, update).IsAcknowledged;
        }

        public bool CancelActivesPasswordResets(Guid userKey)
        {
            var update = Builders<PasswordReset>.Update.Set(x => x.Status, PasswordResetStatusEnum.Used);

            return this.Collection.UpdateOne(x => 
                x.UserKey == userKey && x.Status == PasswordResetStatusEnum.Created, update
            ).IsAcknowledged;
        }
    }
}
