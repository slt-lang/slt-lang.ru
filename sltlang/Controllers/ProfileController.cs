using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using sltlang.Domain.Ports;

namespace sltlang.Controllers
{
    //[Authorize]
    public class ProfileController(ILocaleService locale) : Controller
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
        public IActionResult Index() => NotCulture(out var lang) ? CultureNotFound(lang) : View();

        [OutputCache(VaryByRouteValueNames = ["lng"])]
        public IActionResult CultureNotFound(string lng)
        {
            return RedirectPermanent($"/{lng}");
        }
    }
}
