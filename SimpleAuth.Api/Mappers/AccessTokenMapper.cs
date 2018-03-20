using IpInfo.Api.Client.Models;
using PackUtils;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Models.Filters;
using SimpleAuth.Api.Models.Request;
using SimpleAuth.Api.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using UAUtil.Models;

namespace SimpleAuth.Api.Mappers
{
    public static class AccessTokenMapper
    {
        public static GetAccessTokenResponse Map(AccessToken accessToken, User user)
        {
            GetAccessTokenResponse response = new GetAccessTokenResponse
            {
                UserKey = accessToken.UserKey,
                User = UserMapper.Map(user),
                Token = accessToken.Token,
                IpInfo = accessToken.IpInfo,
                Ip = accessToken.Ip,
                DeviceInfo = accessToken.DeviceInfo,
                CreateDate = accessToken.CreateDate
            };

            return response;
        }

        public static SearchAccessTokensFilters Map(SearchSessionsRequest request)
        {
            SearchAccessTokensFilters filters = new SearchAccessTokensFilters
            {
                UserKey = request.UserKey,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                SortField = request.SortField,
                SortMode = request.SortMode.ToString()
            };

            return filters;
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

        public static SearchResponse<GetAccessTokenResponse> Map(SearchContainer<AccessToken> tokens, string url = "")
        {
            SearchResponse<GetAccessTokenResponse> tokensResponse = new SearchResponse<GetAccessTokenResponse>
            {
                Paging = new PagedList.PagedList(url, tokens.Total, tokens.PageNumber, tokens.PageSize),
                Items = new List<GetAccessTokenResponse>()
            };

            if (tokens.Items != null && tokens.Items.Any())
            {
                foreach (var token in tokens.Items)
                {
                    tokensResponse.Items.Add(AccessTokenMapper.Map(token, null));
                }
            }

            return tokensResponse;
        }
    }
}
