using PackUtils;
using SimpleAuth.Api.Models.Enums;
using System;

namespace SimpleAuth.Api.Models
{
    public class PasswordReset
    {
        public Guid UserKey { get; set; }

        public string Token { get; set; }

        public PasswordResetStatusEnum Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public long ExpiresInMs { get; set; }
        
        public static PasswordReset New(Guid userKey, int expiresInHours)
        {
            var now = DateTime.UtcNow;
            PasswordReset obj = new PasswordReset
            {
                UserKey = userKey,
                Status = PasswordResetStatusEnum.Created,
                CreateDate = now,
                ExpiresInMs = expiresInHours * 60 * 60 * 1000,
                Token = HashUtility.GenerateRandomSha256(),
                ExpirationDate = now.AddHours(expiresInHours)
            };
            
            return obj;
        }
    }
}
