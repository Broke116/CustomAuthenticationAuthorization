using System.Web.Mvc;
using AuthenticationAuthorization.DAL.Security;

namespace AuthenticationAuthorization.Controllers
{
    public class UserController : BaseController
    {
        [CustomAuthorize(Roles = "User")]
        public ActionResult Index()
        {
            return View();
        }
    }
}