using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SLThree;
using SLThree.Extensions;
using Specification;
using System.Collections.Concurrent;

namespace sltlang.Controllers
{
    public class SpecificationController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILocaleService _locale;
        public static readonly ConcurrentDictionary<string, Type> Types = new ConcurrentDictionary<string, Type>(new Dictionary<string, Type>()
        {
            { "baseexpression", typeof(BaseExpression) },
            { "binaryoperator", typeof(BinaryOperator) },
        });
        public static readonly Type[] DefaultTypes = new Type[]
        {
            typeof(SLThree.ExecutionContext),
        };
        private static readonly ConcurrentDictionary<string, Type> dtypes = new ConcurrentDictionary<string, Type>(DefaultTypes.Select(x => new KeyValuePair<string, Type>(x.Name.ToLower(), x)));

        public SpecificationController(ILogger<HomeController> logger, ILocaleService locale)
        {
            _logger = logger;
            _locale = locale;
        }

        [OutputCache(VaryByRouteValueNames = ["culture"])]
        public IActionResult Index(string article)
        {
            article = article.ToLower();
            var Language = (string)(HttpContext.GetRouteValue("culture") ?? "");
            if (_locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = _locale.Locales[Language];
                if (Types.ContainsKey(article))
                    return View(article, Types[article]);
                if (dtypes.ContainsKey(article))
                    return View(dtypes[article]);
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
