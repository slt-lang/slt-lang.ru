using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Common.AuthService.Contracts
{
    public class CreateInviteLinkRequest
    {
        public string InviteLink { get; set; } = default!;
        public int InviteLinkTtl { get; set; }
        public string? RedirectUrl { get; set; }
    }

    public class CreateInviteLinkResponse
    {
        public int InviteId { get; set; }
        public CreateInviteResult Result { get; set; }
    }
}
