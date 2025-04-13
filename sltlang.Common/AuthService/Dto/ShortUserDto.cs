namespace sltlang.Common.AuthService.Dto
{
    public class ShortUserDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; } = default!;
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// Активен ли аккаунт пользователя
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Является ли пользователь шаблоном для других пользователей
        /// </summary>
        public bool IsTemplate { get; set; }
    }
}
