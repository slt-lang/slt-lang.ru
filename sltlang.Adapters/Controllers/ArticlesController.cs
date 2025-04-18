using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sltlang.Adapters.Extensions;
using sltlang.Common.ArticleService.Contracts;
using sltlang.Common.AuthService.Contracts;
using sltlang.Common.Common.Extensions;
using sltlang.Common.TelegramService;
using sltlang.Domain;
using sltlang.Domain.Ports;

namespace sltlang.Adapters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticlesController(IArticleLogic articleLogic, Config config, TelegramService telegramService) : ControllerBase
    {
        [Authorize]
        [HttpPost("Upsert")]
        public async Task<IActionResult> UpsertArticle([FromForm] UpsertArticleRequest request) //[FromForm] InviteLinkDto request)
        {
            await articleLogic.UpsertArticle(new Common.ArticleService.Models.ArticleDto()
            {
                Name = request.Name,
                Title = request.Title,
                CultureKey = request.CultureKey,
                Content = request.Content,
                UserId = HttpContext.User.GetUserId(),
            });

            return Redirect($"{request.RedirectUrl}");
        }
    }
}
