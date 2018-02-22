namespace SimpleAuth.Api.Models.Request
{
    public class UsePasswordResetRequest
    {
        public string Token { get; set; }

        public string Password { get; set; }
    }
}
