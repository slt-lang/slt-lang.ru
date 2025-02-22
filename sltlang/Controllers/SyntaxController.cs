using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SLThree;
using SLThree.Extensions;
using sltlang.Domain.Ports;
using Specification;
using System.Collections.Concurrent;

namespace sltlang.Controllers
{
    public class SyntaxController(ILocaleService locale, ILogger<HomeController> logger, ISyntaxPageStorage syntaxPageStorage) : Controller
    {
        [OutputCache(VaryByRouteValueNames = ["culture"])]
        public IActionResult Index(string article)
        {
            article = article.ToLower();
            var Language = (string)(HttpContext.GetRouteValue("culture") ?? "");
            if (locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = locale.Locales[Language];
                var pages = syntaxPageStorage.GetPages().Where(x => x.CultureKey == Language);
                var page = pages.FirstOrDefault(x => x.Name == article);
                if (page != null)
                    return View(page);
                //if (Article.ExecutablesSpecification.ContainsKey(article))
                //    return View(Article.ExecutablesSpecification[article]);
                return ArticleNotFound(article);
            }
            return CultureNotFound(Language);
        }

        [OutputCache(VaryByRouteValueNames = ["culture"])]
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
