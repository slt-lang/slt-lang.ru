using sltlang.Common.AuthService.Dto;

namespace sltlang.Common.AuthService.Contracts
{
    public class RegistrationRequest
    {
        public string Invite { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string PasswordConfirm { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string SuccessRedirectUrl { get; set; } = default!;
        public string FailedRedirectUrl { get; set; } = default!;
    }

    public class RegistrationResponse
    {
        public ShortUserDto TemplateUser { get; set; } = default!;
        public UserDto User { get; set; } = default!;
        public string AccessToken { get; set; } = default!;
    }
}
