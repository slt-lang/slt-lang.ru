using sltlang.Common.AuthService.Contracts;
using sltlang.Common.AuthService.Dto;
using sltlang.Domain.Ports;

namespace sltlang.Domain.Logic
{
    public class InviteLogic(IHttpService httpService) : IInviteLogic
    {
        const string InviteService = "AuthService";

        public async Task<CreateInviteLinkResponse> CreateInviteLink(InviteLinkDto inviteLinkDto)
        {
            var (success, response) = await httpService.JsonRequest<InviteLinkDto, CreateInviteLinkResponse>(InviteService, $"invites/new", [], HttpMethod.Post, inviteLinkDto, TimeSpan.FromSeconds(7));
            return success ? response! : new() { Result = CreateInviteResult.UnknownError };
        }
    }
}
