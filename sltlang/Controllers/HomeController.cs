using Microsoft.AspNetCore.Mvc;
using Specification;
using sltlang.Models;
using System.Diagnostics;
using SLThree.sys;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.OutputCaching;

namespace sltlang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILocaleService _locale;

        public HomeController(ILogger<HomeController> logger, ILocaleService locale)
        {
            _logger = logger;
            _locale = locale;
        }

        private bool NotCulture(out string Language)
        {
            Language = (string)(HttpContext.GetRouteValue("culture") ?? "");
            if (_locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = _locale.Locales[Language];
                return false;
            }
            return true;
        }

        [OutputCache(VaryByRouteValueNames = ["culture"])]
        public IActionResult Index() => NotCulture(out var lang) ? CultureNotFound(lang) : View();

        [OutputCache(VaryByRouteValueNames = ["culture"])]
        public IActionResult Faq() => NotCulture(out var lang) ? CultureNotFound(lang) : View();

        [OutputCache(VaryByRouteValueNames = ["culture"])]
        public IActionResult Syntax() => NotCulture(out var lang) ? CultureNotFound(lang) : View();

        [OutputCache(VaryByRouteValueNames = ["culture"])]
        public IActionResult Info() => NotCulture(out var lang) ? CultureNotFound(lang) : View();

        [OutputCache(VaryByRouteValueNames = ["culture", "statusCode"])]
        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            return View(new ErrorViewModel { StatusCode = statusCode, OriginalPath = feature?.OriginalPath });
        }

        [OutputCache(VaryByRouteValueNames = ["lng"])]
        public IActionResult CultureNotFound(string lng)
        {
            return View("CultureNotFound", lng);
        }
    }
}
