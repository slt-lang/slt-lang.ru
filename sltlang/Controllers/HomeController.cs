using Microsoft.AspNetCore.Mvc;
using Specification;
using sltlang.Models;
using System.Diagnostics;
using SLThree.sys;
using Microsoft.AspNetCore.Diagnostics;

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

        public IActionResult Index()
        {
            var Language = (string)(HttpContext.GetRouteValue("culture") ?? "");
            if (_locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = _locale.Locales[Language];
                return View();
            }
            return CultureNotFound(Language);
        }

        public IActionResult Faq()
        {
            var Language = (string)(HttpContext.GetRouteValue("culture") ?? "");
            if (_locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = _locale.Locales[Language];
                return View();
            }
            return CultureNotFound(Language);
        }
        public IActionResult Syntax()
        {
            var Language = (string)(HttpContext.GetRouteValue("culture") ?? "");
            if (_locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = _locale.Locales[Language];
                return View();
            }
            return CultureNotFound(Language);
        }

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            return View(new ErrorViewModel { StatusCode = statusCode, OriginalPath = feature?.OriginalPath });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult CultureNotFound(string lng)
        {
            return View("CultureNotFound", lng);
        }
    }
}
