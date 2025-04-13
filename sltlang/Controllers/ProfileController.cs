using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using sltlang.Adapters.Extensions;
using sltlang.Domain.Ports;
using sltlang.Models;
using System.Security.Claims;

namespace sltlang.Controllers
{
    public class ProfileController(IAuthLogic authLogic, IUserLogic userLogic, ILocaleService locale) : Controller
    {
        private bool NotCulture(out string Language)
        {
            Language = (string)(HttpContext.GetRouteValue("culture") ?? "ru");
            if (locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = locale.Locales[Language];
                return false;
            }
            return true;
        }

        [HttpGet("{culture=ru}/profile")]
        public async Task<IActionResult> Index([FromQuery] int? userId, string? result = null)
        {
            if (NotCulture(out var lang))
            {
                return CultureNotFound(lang);
            }

            var model = new ProfilePage()
            {
                LastNotification = result!,
            };

            if (userId == null)
            {
                if (HttpContext.User?.Identity?.IsAuthenticated ?? false)
                {
                    userId = HttpContext.User.GetUserId();
                    model.Own = true;
                }
            }

            if (userId == null)
                return View(model);

            var user = await userLogic.GetUser(userId.Value);
            if (user == null)
            {
                model.LastNotification = "Result_UserNotFound";
                return View(model);
            }

            model.FullUser = user;

            return View(model);
        }

        [OutputCache(VaryByRouteValueNames = ["lng"])]
        public IActionResult CultureNotFound(string lng)
        {
            return RedirectPermanent($"/{lng}");
        }
    }
}
