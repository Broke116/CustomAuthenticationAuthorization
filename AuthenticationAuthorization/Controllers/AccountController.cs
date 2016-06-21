using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AuthenticationAuthorization.DAL;
using AuthenticationAuthorization.Models.Viewmodels;
using Newtonsoft.Json;

namespace AuthenticationAuthorization.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DataContext _context = new DataContext();

        public ActionResult Index()
        {
            return HttpContext.User.Identity.IsAuthenticated ? RedirectToAction("Index","Home") : RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    var roles = user.Roles.Select(m => m.RoleName).ToArray();
                    var principalModel = new PrincipalModel();
                    principalModel.UserId = user.UserId;
                    principalModel.FirstName = user.FirstName;
                    principalModel.LastName = user.LastName;
                    principalModel.Roles = roles;

                    var userData = JsonConvert.SerializeObject(principalModel);
                    var authenticationTicket = new FormsAuthenticationTicket(
                        1,
                        user.Email,
                        DateTime.Now, 
                        DateTime.Now.AddHours(1),
                        model.RememberMe,
                        userData);

                    var encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
                    var httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(httpCookie);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index","Admin");
                    }
                    else if (roles.Contains("User"))
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Incorrect username and/or password");
            }

            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }
    }
}