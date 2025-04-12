using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sltlang.Adapters.Extensions;
using sltlang.Common.AuthService.Contracts;
using sltlang.Common.TelegramService;
using sltlang.Domain;
using sltlang.Domain.Logic;
using sltlang.Domain.Ports;
using System.Text;
using System.Text.Json;

namespace sltlang.Adapters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthLogic authLogic, Config config, TelegramService telegramService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequest loginRequest)
        {
            var resp = default(LoginResponse);
            try
            {
                resp = await authLogic.Login(loginRequest);
            }
            catch (Exception ex)
            {
                return Redirect(loginRequest.RedirectUrl + "?result=authserviceerror");
            }

            if (resp?.AccessToken != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = config.UseSecureCookie,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.Now.AddMinutes(30)
                };

                HttpContext.Response.Cookies.Append("JwtToken", resp.AccessToken, cookieOptions);
            }

            return Redirect(loginRequest.RedirectUrl + (resp?.AccessToken == null ? "?result=wrong" : ""));
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromForm] string redirectUrl)
        {
            HttpContext.Response.Cookies.Delete("JwtToken");
            return Redirect(redirectUrl);
        }

        [HttpGet("token")]
        [Authorize]
        public IActionResult Token()
        {
            return Ok(HttpContext.Request.Cookies["JwtToken"]);
        }

        [HttpGet("permissions")]
        [Authorize]
        public IActionResult GetPermissions()
        {
            return Ok(string.Join(", ", HttpContext.User.GetPermissions()));
        }
    }
}
