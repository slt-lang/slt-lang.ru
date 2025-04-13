using sltlang.Common.AuthService.Enums;
using System;
using System.Collections.Generic;

namespace sltlang.Common.AuthService.Dto
{
    public class UserDto : ShortUserDto
    {
        public Dictionary<Variable, string> Variables { get; set; } = default!;
        public Dictionary<Permission, PermissionDto> Permissions { get; set; } = default!;
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// Кем приглашён
        /// </summary>
        public ShortUserDto? InvitedBy { get; set; } = default!;
        public ShortInviteLinkDto[] InviteLinks { get; set; } = default!;
    }
}
