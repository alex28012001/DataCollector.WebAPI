using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataCollector.WebAPI.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public IActionResult OnGetAsync(string returnUrl = null)
        {
            var provider = "Microsoft";
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Page("./Login", pageHandler: "Callback", values: new { returnUrl }),          
            };

            return new ChallengeResult(provider, authenticationProperties);
        }
        public async Task<IActionResult> OnGetCallbackAsync()
        {
            var user = this.User.Identities.FirstOrDefault();
            if (user.IsAuthenticated)
            {
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    RedirectUri = this.Request.Host.Value
                };

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(user),
                authProperties);
            }
            return LocalRedirect("/users");
        }
    }
}
