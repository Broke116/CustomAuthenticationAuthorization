using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using AuthenticationAuthorization.DAL.Security;
using AuthenticationAuthorization.Models.Viewmodels;
using Newtonsoft.Json;

namespace AuthenticationAuthorization
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null)
                {
                    var serializeModel = JsonConvert.DeserializeObject<PrincipalModel>(authTicket.UserData);
                    var newUser = new CustomPrincipal(authTicket.Name)
                    {
                        UserId = serializeModel.UserId,
                        FirstName = serializeModel.FirstName,
                        LastName = serializeModel.LastName,
                        Roles = serializeModel.Roles
                    };

                    HttpContext.Current.User = newUser;
                }
            }

        }
    }
}
