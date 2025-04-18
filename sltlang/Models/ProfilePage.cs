using sltlang.Common.ArticleService.Models;
using sltlang.Common.AuthService.Dto;
using sltlang.Common.Common;

namespace sltlang.Models
{
    public class ProfilePage
    {
        public string? LastNotification { get; set; } = default!;
        public string[] LastEditedArticles { get; set; } = [];
        public bool Own { get; set; }
        public UserDto FullUser { get; set; } = default!;
        public ArticleDto[] UserEditedArticles { get; set; } = [];
    }
}
