using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthenticationAuthorization.DAL.Security;

namespace AuthenticationAuthorization.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        [Authorize]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}