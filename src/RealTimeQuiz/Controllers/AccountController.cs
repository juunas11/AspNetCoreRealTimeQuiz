using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace RealTimeQuiz.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignIn()
        {
            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Quiz")
                },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult SignOut()
        {
            //Get absolute URL
            string returnUrl = Url.Action(
                action: nameof(SignedOut),
                controller: "Account",
                values: null,
                protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = returnUrl
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(
                    actionName: "Index",
                    controllerName: "Home");
            }
            return View();
        }
    }
}
