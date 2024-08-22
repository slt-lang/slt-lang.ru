using Microsoft.AspNetCore.Mvc;
using Specification;

namespace sltlang.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILocaleService _locale;

        public ArticleController(ILogger<HomeController> logger, ILocaleService locale)
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
                if (Specification.Article.OtherArticles.ContainsKey(article))
                    return View(Specification.Article.OtherArticles[article]);
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
