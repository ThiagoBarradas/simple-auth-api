namespace SimpleAuth.Api.Models.Response
{
    public class GetAccessTokenResponse : AccessToken
    {
        public GetUserResponse User { get; set; }
    }
}
