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
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var resp = default(LoginResponse);
            try
            {
                resp = await authLogic.Login(request);
            }
            catch (Exception ex)
            {
                return Redirect(request.RedirectUrl + "?result=authserviceerror");
            }

            if (resp?.AccessToken != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = config.UseSecureCookie,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.Now.AddMinutes(30)
                };

                HttpContext.Response.Cookies.Append("JwtToken", resp.AccessToken, cookieOptions);
            }

            return Redirect(request.RedirectUrl + (resp?.AccessToken == null ? "?result=wrong" : ""));
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromForm] string redirectUrl)
        {
            HttpContext.Response.Cookies.Delete("JwtToken");
            return Redirect(redirectUrl);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegistrationRequest request)
        {
            var resp = default(RegistrationResponse);
            try
            {
                resp = await authLogic.Registration(request);
            }
            catch (Exception ex)
            {
                return Redirect(request.FailedRedirectUrl + "?result=authserviceerror");
            }

            if (resp?.AccessToken != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = config.UseSecureCookie,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.Now.AddMinutes(30)
                };

                telegramService.FireForgetLog(new Common.TelegramService.Models.TelegramMessage()
                {
                    Message = $"Зарегистрирован новый пользователь {resp.User.Id} по приглашению пользователя {resp.User.InvitedBy.Id} по шаблону {resp.TemplateUser.Username}",
                    Tags = ["registration"]
                });

                HttpContext.Response.Cookies.Append("JwtToken", resp.AccessToken, cookieOptions);
                return Redirect(request.SuccessRedirectUrl);
            }

            return Redirect(request.FailedRedirectUrl + "?result=wrong");
        }

        [HttpGet("permissions")]
        [Authorize]
        public IActionResult GetPermissions()
        {
            return Ok(string.Join(", ", HttpContext.User.GetPermissions()));
        }
    }
}
