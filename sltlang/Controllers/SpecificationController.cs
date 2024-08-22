using Microsoft.AspNetCore.Mvc;
using SLThree;
using Specification;

namespace sltlang.Controllers
{
    public class SpecificationController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILocaleService _locale;


        public SpecificationController(ILogger<HomeController> logger, ILocaleService locale)
        {
            _logger = logger;
            _locale = locale;
        }

        public IActionResult Index(string article)
        {
            var Language = (string)(HttpContext.GetRouteValue("culture") ?? "");
            if (_locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = _locale.Locales[Language];
                if (Article.ExecutablesSpecification.ContainsKey(article))
                    return View(Article.ExecutablesSpecification[article]);
                return ArticleNotFound(article);
            }
            return CultureNotFound(Language);
        }

        public IActionResult ArticleNotFound(string article)
        {
            return View("ArticleNotFound", model: article);
        }

        public IActionResult CultureNotFound(string lng)
        {
            return RedirectPermanent($"/{lng}");
        }
    }
}
