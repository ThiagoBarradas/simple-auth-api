using Nancy;
using SimpleAuth.Api.Modules.Interface;
using SimpleAuth.Api.Utilities;

namespace SimpleAuth.Api.Controller
{
    public class HomeController : BaseController
    {
        public HomeController(ISecurityModule securityModule)
            : base(securityModule)
        {
            this.Get("", args => this.Home());
        }

        public object Home()
        {
            return Response.AsText(ApplicationUtility.GetApplicationTitle());
        }
    }
}
