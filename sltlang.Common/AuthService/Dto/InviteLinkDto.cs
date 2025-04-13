using sltlang.Common.AuthService.Enums;
using System.Collections.Generic;

namespace sltlang.Common.AuthService.Dto
{
    public class InviteLinkDto : ShortInviteLinkDto
    {
        /// <summary>
        /// Заданные создателем ссылки права для пользователя
        /// </summary>
        public List<PermissionDto> Permissions { get; set; } = default!;
        /// <summary>
        /// Заданные создателем ссылки переменные для пользователя
        /// </summary>
        public Dictionary<Variable, string> Variables { get; set; } = default!;
    }
}
