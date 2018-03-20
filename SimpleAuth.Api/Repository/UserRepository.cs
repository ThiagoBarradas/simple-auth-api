using MongoDB.Bson;
using MongoDB.Driver;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Filters;
using SimpleAuth.Api.Repository.Interface;
using SimpleAuth.Api.Repository.QueryBuilders;
using SimpleAuth.Api.Utilities.Interface;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleAuth.Api.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public const string COLLECTION_NAME = nameof(User);

        public UserRepository(IConfigurationUtility configurationUtility) 
            : base(configurationUtility, UserRepository.COLLECTION_NAME) { }
        
        public bool ExistsEmail(string email)
        {
            return this.Collection.AsQueryable().Where(u => u.Contacts.Email.ToLower() == email.ToLowerInvariant().Trim()).Any();
        }

        public bool CreateOrUpdateUser(User user)
        {
            return this.Collection.ReplaceOne(
                 new BsonDocument("_id", user.UserKey),
                 user,
                 new UpdateOptions { IsUpsert = true }
            ).IsAcknowledged;
        }

        public User GetUser(Guid userKey)
        {
            var user = this.Collection.Find(u => u.UserKey == userKey).FirstOrDefault();

            return user;
        }

        public User GetUser(string email)
        {
            email = email.ToLowerInvariant().Trim();
            var user = this.Collection.AsQueryable().Where(u => u.Contacts.Email.ToLower() == email).FirstOrDefault();

            return user;
        }

        public User GetActiveUser(Guid userKey)
        {
            var user = this.Collection.AsQueryable().Where(u => u.UserKey == userKey &&
                                                  u.Security.IsBlocked == false &&
                                                  u.Security.EmailConfirmed == true).FirstOrDefault();
            return user;
        }

        public User GetActiveUser(string email, string passwordHash)
        {
            email = email.ToLowerInvariant().Trim();
            var user = this.Collection.AsQueryable().Where(u => u.Contacts.Email.ToLower() == email &&
                                                  u.Security.PasswordHash == passwordHash &&
                                                  u.Security.IsBlocked == false &&
                                                  u.Security.EmailConfirmed == true).FirstOrDefault();
            return user;
        }

        public User GetActiveUser(string email)
        {
            email = email.ToLowerInvariant().Trim();
            var user = this.Collection.AsQueryable().Where(u => u.Contacts.Email.ToLower() == email &&
                                                 u.Security.IsBlocked == false &&
                                                 u.Security.EmailConfirmed == true).FirstOrDefault();
            return user;
        }

        public bool UpdateUserPassword(Guid userKey, string passwordHash)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserKey, userKey);
            var update = Builders<User>.Update.Set(u => u.Security.PasswordHash, passwordHash);

            return this.Collection.UpdateOne(filter, update).IsAcknowledged;
        }

        public bool ConfirmUserEmail(string emailConfirmationToken)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Security.EmailConfirmationToken, emailConfirmationToken);
            var update = Builders<User>.Update.Set(u => u.Security.EmailConfirmed, true)
                                              .Set(u => u.Security.EmailConfirmationToken, null);

            return this.Collection.UpdateOne(filter, update).ModifiedCount > 0;
        }

        public SearchContainer<User> ListUsers(SearchUsersFilters filters)
        {
            FilterDefinition<User> filter = this.BuildUsersQuery(filters);

            var result = this.Collection.Find(filter)
                .WithSorting(filters)
                .WithPaging(filters).ToList();

            return new SearchContainer<User>()
            {
                Items = result,
                Total = result.Count,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize
            };
        }

        private FilterDefinition<User> BuildUsersQuery(SearchUsersFilters filters)
        {
            FilterDefinition<User> filter = null;

            if (filters.IsActive != null)
            {
                filter = filter.FilterJoin(Builders<User>.Filter.Eq(x => x.Security.IsBlocked, !filters.IsActive));
            }

            if (string.IsNullOrWhiteSpace(filters.Role) == false && string.IsNullOrWhiteSpace(filters.PermissionKey) == false)
            {
                filter = filter.FilterJoin(Builders<User>.Filter
                    .ElemMatch(x => x.Roles, x => x.Keys.Contains(filters.PermissionKey) & x.Type == filters.Role));
            }
            else if (string.IsNullOrWhiteSpace(filters.Role) == false)
            {
                filter = filter.FilterJoin(Builders<User>.Filter.ElemMatch(x => x.Roles, x => x.Type == filters.Role));
            }
            else if (string.IsNullOrWhiteSpace(filters.PermissionKey) == false)
            {
                filter = filter.FilterJoin(Builders<User>.Filter.ElemMatch(x => x.Roles, x => x.Keys.Contains(filters.PermissionKey)));
            }

            if (string.IsNullOrWhiteSpace(filters.Keywords) == false)
            {
                var regexFilter = Regex.Escape(filters.Keywords);
                var bsonRegex = new BsonRegularExpression(filters.Keywords, "i");

                var name = Builders<User>.Filter.Regex(x => x.Name, bsonRegex);
                var email = Builders<User>.Filter.Regex(x => x.Contacts.Email, bsonRegex);
                var phone = Builders<User>.Filter.Regex(x => x.Contacts.Phone, bsonRegex);
                var company = Builders<User>.Filter.Regex(x => x.Company, bsonRegex);

                var keywordFilter = (name | company | email | phone);

                filter = filter.FilterJoin(keywordFilter);
            }

            if (filter == null) filter = Builders<User>.Filter.Empty;

            return filter;
        }
    }
}