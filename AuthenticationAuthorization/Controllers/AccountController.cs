using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AuthenticationAuthorization.DAL;
using AuthenticationAuthorization.Models;
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    var roles = user.Roles.Select(m => m.RoleName).ToArray();
                    var principalModel = new PrincipalModel
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = roles
                    };

                    var userData = JsonConvert.SerializeObject(principalModel);
                    var authenticationTicket = new FormsAuthenticationTicket(
                        1,
                        user.Username,
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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var userExists = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Email == model.Email);
                if (userExists == null)
                {
                    var existingRole = _context.Roles.FirstOrDefault(m => m.RoleName == "User");

                    if (existingRole != null)
                    {
                        // we create a new user object to put our incoming datas.
                        var user = new User
                        {
                            Username = model.Username,
                            Email = model.Email,
                            Password = model.Password,
                            FirstName = model.Firstname,
                            LastName = model.Lastname,
                            IsActive = true,
                            CreateDate = DateTime.Now,
                        };
                        // add user to existing role(user).
                        existingRole.Users.Add(user);
                    }
                    _context.SaveChanges();
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }
    }
}