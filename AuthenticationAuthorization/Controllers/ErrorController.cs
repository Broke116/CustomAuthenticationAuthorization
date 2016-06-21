using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthenticationAuthorization.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult AccessDenied()
        {
            return null; // MAKE ERROR PAGE SEND PARAMETER, CONTROL ACCESS DENIED OR OTHER ERROR OCCURS
        }
    }
}