using SimpleAuth.Api.Models;
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

        List<AccessToken> GetAllAccessTokens(Guid userKey);

        AccessToken GetAccessToken(Guid userKey, UserAgentDetails deviceInfo, string ip);
    }
}
