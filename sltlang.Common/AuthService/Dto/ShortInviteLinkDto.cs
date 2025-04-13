using System;

namespace sltlang.Common.AuthService.Dto
{
    public class ShortInviteLinkDto
    {
        /// <summary>
        /// Id ссылки
        /// </summary>
        public int LinkId { get; set; }
        /// <summary>
        /// Id пользователя инициировавшего создание ссылки
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Путь ссылки
        /// </summary>
        public string Link { get; set; } = default!;
        /// <summary>
        /// Время истечения срока действия
        /// </summary>
        public DateTime Ttl { get; set; }
        /// <summary>
        /// Права какого пользователя скопирует новый пользователь
        /// </summary>
        public int? InheritanceUserId { get; set; } = default!;
        /// <summary>
        /// Имя пользователя-шаблона, если InheritanceUserId это шаблон
        /// </summary>
        public string? TemplateUsername { get; set; }
    }
}
