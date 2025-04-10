using System;
using System.Collections.Generic;

namespace sltlang.Common.AuthService.Dto
{
    public class InviteLinkDto
    {
        /// <summary>
        /// Id пользователя инициировавшего создание ссылки
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Возможность многократного использования
        /// </summary>
        public bool Reusable { get; set; }
        /// <summary>
        /// Использования ссылки
        /// </summary>
        public int Uses { get; set; }
        /// <summary>
        /// Предел использования
        /// </summary>
        public int MaxUses { get; set; }
        /// <summary>
        /// Путь ссылки
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// Время истечения срока действия
        /// </summary>
        public DateTime Ttl { get; set; }
        /// <summary>
        /// Какие права получит новый пользователь
        /// </summary>
        public List<PermissionDto> Permissions { get; set; } = default!;
    }
}
