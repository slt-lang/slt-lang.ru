using Microsoft.AspNetCore.Mvc;
using Specification;
using sltlang.Models;
using System.Diagnostics;
using SLThree.sys;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.OutputCaching;
using sltlang.Domain.Ports;
using sltlang.Adapters.Adapters;
using SLThree;
using Microsoft.AspNetCore.Mvc.Routing;
using sltlang.Common.AuthService.Contracts;
using System.Text.Json;

namespace sltlang.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IAuthLogic authLogic, ILocaleService locale) : Controller
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

        [OutputCache(VaryByRouteValueNames = ["culture"], Duration = 60)]
        public IActionResult Index() => NotCulture(out var lang) ? CultureNotFound(lang) : View();

        [OutputCache(VaryByRouteValueNames = ["culture"], Duration = 60)]
        public IActionResult Articles() => NotCulture(out var lang) ? CultureNotFound(lang) : View();

        [OutputCache(VaryByRouteValueNames = ["culture"], Duration = 60)]
        public IActionResult Syntax()
        {
            if (NotCulture(out var lang))
            {
                return CultureNotFound(lang);
            }
            else
            {
                ViewData["ListedTypes"] = SampleMaker.ListedTypes.Where(x => typeof(SLThree.ExecutionContext.IExecutable).IsAssignableFrom(x)).OrderBy(x => x.Name).Select(x => ($"{lang}/syntax/{x.Name}", x.Name)).ToArray();
                return View();
            }
        }

        [OutputCache(VaryByRouteValueNames = ["culture"], Duration = 60)]
        public IActionResult Info() => NotCulture(out var lang) ? CultureNotFound(lang) : View();

        [OutputCache(VaryByRouteValueNames = ["culture", "statusCode"])]
        [Route("Error/{statusCode}")]
        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 401)
                return Redirect("~/login");
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            return View(new ErrorViewModel { StatusCode = statusCode, OriginalPath = feature?.OriginalPath });
        }

        [OutputCache(VaryByRouteValueNames = ["lng"])]
        public IActionResult CultureNotFound(string lng)
        {
            return View("CultureNotFound", lng);
        }

        public IActionResult Login() => NotCulture(out var lang) ? CultureNotFound(lang) : View();
        public IActionResult LoginFailed() => NotCulture(out var lang) ? CultureNotFound(lang) : View();
    }
}
