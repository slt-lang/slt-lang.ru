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
        public static readonly ConcurrentDictionary<string, Type> TypesWithArticle = new ConcurrentDictionary<string, Type>(new Dictionary<string, Type>()
        {
            { "baseexpression", typeof(BaseExpression) },
            { "binaryoperator", typeof(BinaryOperator) },
        });
        public static readonly Type[] SupportedTypes = GetSupportedTypes();

        private static Type[] GetSupportedTypes()
        {
            var l = new List<Type>();
            var types = typeof(BaseExpression).Assembly.GetTypes();
            l.Add(typeof(SLThree.ExecutionContext));
            l.Add(typeof(SLThree.ExecutionContext.IExecutable));
            l.Add(typeof(SLThree.ContextWrap));
            l.Add(typeof(SLThree.SourceContext));
            l.AddRange(types.Where(x => x.IsType(typeof(BaseExpression))));
            l.AddRange(types.Where(x => x.IsType(typeof(BaseStatement))));
            return l.Distinct().ToArray();
        }

        private static readonly ConcurrentDictionary<string, Type> TypesWithoutArticle = new ConcurrentDictionary<string, Type>(SupportedTypes.Select(x => new KeyValuePair<string, Type>(x.Name.ToLower(), x)));

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
                if (TypesWithArticle.ContainsKey(article))
                    return View(article, TypesWithArticle[article]);
                if (TypesWithoutArticle.ContainsKey(article))
                    return View(TypesWithoutArticle[article]);
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
