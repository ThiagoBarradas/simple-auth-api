using Newtonsoft.Json;

namespace SimpleAuth.Api.Models.Request
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public string UserAgent{ get; set; }

        [JsonIgnore]
        public string Ip { get; set; }
    }
}
