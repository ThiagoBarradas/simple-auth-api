using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Filters;
using System;
using System.Collections.Generic;
using UAUtil.Models;

namespace SimpleAuth.Api.Repository.Interface
{
    public interface IAuthRepository
    {
        bool DeleteAccessToken(string token);

        bool DeleteAllAccessTokens(Guid userKey, string exceptToken);

        bool CreateAccessToken(AccessToken accessToken);

        AccessToken GetAccessToken(string token);

        SearchContainer<AccessToken> GetAllAccessTokens(SearchAccessTokensFilters filters);

        AccessToken GetAccessToken(Guid userKey, UserAgentDetails deviceInfo, string ip);
    }
}
