using IpInfo.Api.Client.Models;
using PackUtils;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Response;
using System;
using UAUtil.Models;

namespace SimpleAuth.Api.Mappers
{
    public static class AccessTokenMapper
    {
        public static GetAccessTokenResponse Map(AccessToken accessToken, User user)
        {
            GetAccessTokenResponse response = new GetAccessTokenResponse
            {
                UserKey = user.UserKey,
                User = UserMapper.Map(user),
                Token = accessToken.Token,
                IpInfo = accessToken.IpInfo,
                Ip = accessToken.Ip,
                DeviceInfo = accessToken.DeviceInfo,
                CreateDate = accessToken.CreateDate
            };

            return response;
        }

        public static AccessToken Map(UserAgentDetails deviceInfo, GetIpInfoResponse ipInfo, string ip, User user)
        {
            AccessToken accessToken = new AccessToken
            {
                CreateDate = DateTime.UtcNow,
                DeviceInfo = deviceInfo,
                IpInfo = ipInfo,
                Ip = ip,
                Token = HashUtility.GenerateRandomSha256(),
                UserKey = user.UserKey
            };

            return accessToken;
        }
    }
}
