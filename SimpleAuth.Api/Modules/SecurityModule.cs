using Nancy;
using SimpleAuth.Api.Models;
using SimpleAuth.Api.Modules.Interface;
using SimpleAuth.Api.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace SimpleAuth.Api.Modules
{
    public class SecurityModule : ISecurityModule
    {
        private static string AUTH_TYPE = "Bearer";

        public User User { get; private set; }

        public AccessToken AccessToken { get; private set; }

        private IAuthRepository AuthRepository { get; set; }

        private IUserRepository UserRepository { get; set; }

        public SecurityModule(IAuthRepository authRepository, IUserRepository userRepository)
        {
            this.AuthRepository = authRepository;
            this.UserRepository = userRepository;
        }

        public bool Authorize(NancyModule module)
        {

            var authorization = module.Context.Request.Headers.Authorization ?? string.Empty;
            var accessToken = this.AuthRepository.GetAccessToken(authorization);

            if (accessToken == null)
            {
                return false;
            }

            var user = this.UserRepository.GetActiveUser(accessToken.UserKey);
            if (user == null)
            {
                return false;
            }

            this.User = user;
            this.AccessToken = accessToken;
            module.Context.CurrentUser = this.GeneratePrincipal(user, accessToken);

            return true;
        }

        private ClaimsPrincipal GeneratePrincipal(User user, AccessToken accessToken)
        {
            if (user.Roles == null)
            {
                user.Roles = new List<UserRole>();
            }

            var roles = user.Roles.Select(r => r.Type.ToString()).ToArray();

            var identity = new GenericIdentity(user.Name, SecurityModule.AUTH_TYPE);
            identity.TryRemoveClaim(identity.Claims.FirstOrDefault());

            var principal = new GenericPrincipal(identity, roles);

            Claim token = new Claim(ClaimTypes.Authentication, accessToken.Token, "Info", SecurityModule.AUTH_TYPE, SecurityModule.AUTH_TYPE, identity);
            Claim userKey = new Claim(ClaimTypes.NameIdentifier, user.UserKey.ToString(), "Info", SecurityModule.AUTH_TYPE, SecurityModule.AUTH_TYPE, identity);
            Claim email = new Claim(ClaimTypes.Email, user.Contacts.Email, "Info", SecurityModule.AUTH_TYPE, SecurityModule.AUTH_TYPE, identity);

            identity.AddClaim(token);
            identity.AddClaim(userKey);
            identity.AddClaim(email);

            user.Roles.ForEach(role =>
            {
                Claim tempRole = new Claim(ClaimTypes.Role, role.Type.ToString(), "Role", SecurityModule.AUTH_TYPE, SecurityModule.AUTH_TYPE, identity);

                if (role.Keys != null)
                {
                    role.Keys.ForEach(key =>
                    {
                        tempRole.Properties.Add(new KeyValuePair<string, string>(key, role.Type.ToString()));
                    });
                    identity.AddClaim(tempRole);
                }
            });

            return principal;
        }
    }
}
