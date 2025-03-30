using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using sltlang.Domain.Ports;
using Specification;

namespace sltlang.Controllers
{
    public class ArticleController(ILogger<HomeController> logger, ILocaleService locale, IArticleLogic articleLogic) : Controller
    {
        [OutputCache(VaryByRouteValueNames = ["culture"])]
        public async Task<IActionResult> Index(string article)
        {
            var Language = (string)(HttpContext.GetRouteValue("culture") ?? "");
            if (locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = locale.Locales[Language];

                try
                {
                    var articleData = await articleLogic.GetArticle(Language, article);
                    if (articleData != null)
                        return View(articleData);
                }
                catch (Exception e)
                {
                    return ServiceNotAllowed();
                }
                return ArticleNotFound(article);
            }
            return CultureNotFound(Language);
        }

        [OutputCache(VaryByRouteValueNames = ["culture"])]
        public IActionResult ServiceNotAllowed()
        {
            return View("_ServiceNotAllowed");
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
