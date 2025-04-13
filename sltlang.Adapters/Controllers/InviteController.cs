using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sltlang.Adapters.Extensions;
using sltlang.Common.AuthService.Contracts;
using sltlang.Common.AuthService.Enums;
using sltlang.Common.Common;
using sltlang.Common.Common.Extensions;
using sltlang.Common.TelegramService;
using sltlang.Domain;
using sltlang.Domain.Ports;

namespace sltlang.Adapters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InviteController(IInviteLogic inviteLogic, Config config, TelegramService telegramService) : ControllerBase
    {
        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateInviteLink([FromForm] CreateInviteLinkRequest request) //[FromForm] InviteLinkDto request)
        {
            if (!HttpContext.User.HasPermission(Common.AuthService.Enums.Permission.AuthInviteLinks))
            {
                var userId = HttpContext.User.GetUserId();
                var response = await inviteLogic.CreateInviteLink(new Common.AuthService.Dto.InviteLinkDto()
                {
                    InheritanceUserId = null,
                    Permissions = [],
                    TemplateUsername = null,
                    Link = request.InviteLink,
                    Ttl = DateTime.Now.AddMinutes(request.InviteLinkTtl).ToUniversalTime(),
                    UserId = userId,
                    Variables = [],
                });

                if (response.Result == CreateInviteResult.Success)
                {
                    telegramService.FireForgetLog(new Common.TelegramService.Models.TelegramMessage()
                    {
                        Message = $"Пользователь {userId} создал пригласительную ссылку {request.InviteLink} со сроком жизни {request.InviteLinkTtl}",
                        Tags = ["invites"]
                    });
                }

                return Redirect($"{request.RedirectUrl}?result={response?.Result.ToUniversalResult()}");
            }

            return Redirect($"{request.RedirectUrl}?result={CreateInviteResult.PermissionsError.ToUniversalResult()}");
        }
    }
}
