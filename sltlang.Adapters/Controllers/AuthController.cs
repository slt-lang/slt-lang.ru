using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sltlang.Common.AuthService.Contracts;
using sltlang.Domain.Logic;
using sltlang.Domain.Ports;
using System.Text;
using System.Text.Json;

namespace sltlang.Adapters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthLogic authLogic) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            var resp = await authLogic.Login(loginRequest);

            if (resp?.AccessToken != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Only set to true if using HTTPS
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.Now.AddMinutes(30) // Adjust expiration
                };

                HttpContext.Response.Cookies.Append("JwtToken", resp.AccessToken, cookieOptions);
                return Redirect(loginRequest.RedirectUrl);
            }

            return Redirect(loginRequest.RedirectUrl);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromForm] string redirectUrl)
        {
            HttpContext.Response.Cookies.Delete("JwtToken");
            return Redirect(redirectUrl);
        }
    }
}
