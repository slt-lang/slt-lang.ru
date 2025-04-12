using sltlang.Common.AuthService.Dto;

namespace sltlang.Models
{
    public class ProfilePage
    {
        public string LastNotification { get; set; } = default!;
        public string[] LastEditedArticles { get; set; } = [];
        public bool Own { get; set; }
        public UserDto FullUser { get; set; } = default!;
    }
}
