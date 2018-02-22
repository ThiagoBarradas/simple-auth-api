using SimpleAuth.Api.Models;
using System;
using SimpleAuth.Api.Models.Filters;

namespace SimpleAuth.Api.Repository.Interface
{
    public interface IUserRepository
    {
        bool ExistsEmail(string email);

        bool CreateOrUpdateUser(User user);

        User GetUser(Guid userKey);

        User GetUser(string email);

        User GetActiveUser(Guid userKey);

        User GetActiveUser(string email);

        User GetActiveUser(string email, string password);

        bool UpdateUserPassword(Guid userKey, string passwordHash);

        bool ConfirmUserEmail(string emailConfirmationToken);

        SearchContainer<User> ListUsers(SearchUsersFilters filters);
    }
}
