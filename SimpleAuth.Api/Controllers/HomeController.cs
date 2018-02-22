using SimpleAuth.Api.Utilities;
using Nancy;

namespace SimpleAuth.Api.Controller
{
    public class HomeController : BaseController
    {
        public HomeController() 
        {
            this.Get("", args => this.Home());
        }

        public object Home()
        {
            return Response.AsText(ApplicationUtility.GetApplicationTitle());
        }
    }
}
