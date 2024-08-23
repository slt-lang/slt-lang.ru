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

        [OutputCache(VaryByRouteValueNames = ["culture"])]
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

        [OutputCache(VaryByRouteValueNames = ["culture"])]
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

        [OutputCache(VaryByRouteValueNames = ["culture"])]
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
