using sltlang.Common.AuthService.Contracts;
using sltlang.Common.AuthService.Dto;

namespace sltlang.Domain.Ports
{
    public interface IInviteLogic
    {
        Task<CreateInviteLinkResponse> CreateInviteLink(InviteLinkDto inviteLinkDto);
    }
}
