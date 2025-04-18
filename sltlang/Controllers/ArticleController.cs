using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using sltlang.Adapters.Extensions;
using sltlang.Domain.Ports;
using Specification;

namespace sltlang.Controllers
{
    public class ArticleController(ILogger<HomeController> logger, ILocaleService locale, IArticleLogic articleLogic) : Controller
    {
        [OutputCache(VaryByRouteValueNames = ["culture"])]
        public async Task<IActionResult> Index([FromRoute] string article, [FromQuery] string? result)
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

        [Authorize]
        public async Task<IActionResult> Edit([FromRoute] string article, [FromQuery] string? result)
        {
            var permissions = HttpContext.User.GetPermissions();

            var hasCreate = permissions.Contains(Common.AuthService.Enums.Permission.RootPermission)
                || permissions.Contains(Common.AuthService.Enums.Permission.ArticleCreating);

            var hasEdit = permissions.Contains(Common.AuthService.Enums.Permission.RootPermission)
                || permissions.Contains(Common.AuthService.Enums.Permission.ArticleEditing);

            if (!hasCreate && !hasEdit)
            {
                return Forbid();
            }

            var Language = (string)(HttpContext.GetRouteValue("culture") ?? "");

            if (locale.Locales.ContainsKey(Language))
            {
                ViewData["culture"] = locale.Locales[Language];

                try
                {
                    var articleData = await articleLogic.GetArticle(Language, article);
                    if (articleData != null)
                    {
                        if (!hasEdit)
                            return Forbid();
                        return View(articleData);
                    }
                    else
                    {
                        if (!hasCreate)
                            return Forbid();

                        articleData = new Common.ArticleService.Models.ArticleDto()
                        {
                            Name = article,
                            CultureKey = Language,
                        };
                        return View(articleData);
                    }
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
