using Nancy;
using SimpleAuth.Api.Models;

namespace SimpleAuth.Api.Modules.Interface
{
    public interface ISecurityModule
    {
        User User { get; }

        AccessToken AccessToken { get; }
        
        bool Authorize(NancyModule module);
    }
}
