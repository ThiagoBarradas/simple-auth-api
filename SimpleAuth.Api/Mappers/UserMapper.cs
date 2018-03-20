using PackUtils;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Filters;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;
using SimpleAuth.Api.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleAuth.Api.Mappers
{
    public static class UserMapper
    {
        public static GetUserResponse Map(User user)
        {
            if (user == null)
            {
                return null;
            }

            GetUserResponse response = new GetUserResponse
            {
                UserKey = user.UserKey,
                Name = user.Name,
                Company = user.Company,
                CreateDate = user.CreateDate,
                LastUpdate = user.LastUpdate,
                Options = user.Options,
                Contacts = user.Contacts,
                Address = user.Address,
                Roles = user.Roles,
                Security = user.Security
            };

            return response;
        }

        public static SearchUsersFilters Map(SearchUsersRequest request)
        {
            SearchUsersFilters filters = new SearchUsersFilters
            {
                IsActive = request.IsActive,
                Role = request.Role,
                PermissionKey = request.PermissionKey,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                SortField = request.SortField,
                SortMode = request.SortMode.ToString(),
                Keywords = request.Keywords
            };

            return filters;
        }

        public static SearchResponse<GetUserResponse> Map(SearchContainer<User> users, string url = "")
        {
            SearchResponse<GetUserResponse> usersResponse = new SearchResponse<GetUserResponse>
            {
                Paging = new PagedList.PagedList(url, users.Total, users.PageNumber, users.PageSize),
                Items = new List<GetUserResponse>()
            };

            if (users.Items != null && users.Items.Any())
            {
                foreach (var user in users.Items)
                {
                    usersResponse.Items.Add(UserMapper.Map(user));
                }
            }

            return usersResponse;
        }

        public static User Map(CreateUserRequest request, IConfigurationUtility configurationUtility)
        {
            User user = new User
            {
                UserKey = Guid.NewGuid(),
                Name = request.Name,
                Company = request.Company,
                CreateDate = DateTime.UtcNow
            };
            user.LastUpdate = user.CreateDate;
            user.Options = request.Options;
            user.Contacts = request.Contacts;
            user.Contacts.Email = request.Contacts.Email.ToLowerInvariant().Trim();
            user.Address = request.Address;
            user.Roles = request.Roles;

            user.Security = new UserSecurityWithPass()
            {
                PasswordHash = HashUtility.GenerateSha256(request.Password, configurationUtility.HashGap),
                EmailConfirmationToken = HashUtility.GenerateRandomSha256(),
                EmailConfirmed = false,
                IsBlocked = false
            };

            return user;
        }

        public static User Map(User oldUser, UpdateUserRequest request, IConfigurationUtility configurationUtility)
        {
            User newUser = new User
            {
                UserKey = request.UserKey,
                Name = request.Name,
                Company = request.Company,
                CreateDate = oldUser.CreateDate,
                LastUpdate = DateTime.UtcNow,
                Options = request.Options,
                Contacts = request.Contacts,
                Address = request.Address,
                Roles = request.Roles
            };
            newUser.Contacts.Email = request.Contacts.Email.ToLowerInvariant().Trim();

            var passwordHash = (request.Password == null) 
                ? oldUser.Security.PasswordHash 
                : HashUtility.GenerateSha256(request.Password, configurationUtility.HashGap);

            newUser.Security = oldUser.Security;
            newUser.Security.PasswordHash = passwordHash;

            return newUser;
        }
    }
}
